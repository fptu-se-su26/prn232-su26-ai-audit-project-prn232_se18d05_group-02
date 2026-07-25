using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WanderXServer.BusinessObject;
using WanderXServer.DataAccessLayer;
using WanderXServer.Dtos.Recommendations;

namespace WanderXServer.Services;

public sealed class RecommendationService
{
    private static readonly HashSet<string> EligibleTourStatuses = new(StringComparer.OrdinalIgnoreCase) { "Published", "Open", "Active" };
    private static readonly HashSet<string> EligibleDepartureStatuses = new(StringComparer.OrdinalIgnoreCase) { "Assigned", "Confirmed" };
    private static readonly HashSet<string> CapacityBookingStatuses = new(StringComparer.OrdinalIgnoreCase) { "Paid", "Confirmed" };
    private readonly WanderXDbContext _db;
    private readonly RecommendationOptions _options;

    public RecommendationService(WanderXDbContext db, IOptions<RecommendationOptions> options)
    {
        _db = db;
        _options = options.Value;
    }

    public async Task<RecommendationPageResponse> GetForUserAsync(Guid? userId, RecommendationQuery query)
    {
        ValidateQuery(query);
        var quiz = userId.HasValue
            ? await _db.TravelStyleQuizResults.AsNoTracking().Where(x => x.UserId == userId.Value).OrderByDescending(x => x.CompletedAt).FirstOrDefaultAsync()
            : null;
        var candidates = await BuildCandidatesAsync(query, quiz);
        var ordered = ApplySort(candidates, query.Sort).ToList();
        var total = ordered.Count;
        var items = ordered.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToList();
        return new RecommendationPageResponse
        {
            HasQuizResult = quiz != null,
            PrimaryStyle = quiz?.DominantStyle,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = total,
            TotalPages = total == 0 ? 0 : (int)Math.Ceiling(total / (double)query.PageSize),
            Message = total == 0 ? "Không có tour phù hợp với bộ lọc hiện tại." : quiz == null ? "Hãy làm Travel Style Quiz để nhận gợi ý cá nhân hóa hơn." : null,
            Items = items
        };
    }

    public async Task<RecommendedTourResponse?> GetRandomAsync(Guid? userId, RecommendationQuery query)
    {
        ValidateQuery(query);
        var quiz = userId.HasValue
            ? await _db.TravelStyleQuizResults.AsNoTracking().Where(x => x.UserId == userId.Value).OrderByDescending(x => x.CompletedAt).FirstOrDefaultAsync()
            : null;
        var items = await BuildCandidatesAsync(query, quiz);
        return items.Count == 0 ? null : items[RandomNumberGenerator.GetInt32(items.Count)];
    }

    public static IReadOnlyList<RecommendationStyleResponse> GetStyles() => new[]
    {
        new RecommendationStyleResponse { Code = "Adventure", DisplayName = "Phiêu lưu" },
        new RecommendationStyleResponse { Code = "Cultural", DisplayName = "Văn hóa" },
        new RecommendationStyleResponse { Code = "Relaxation", DisplayName = "Nghỉ dưỡng" },
        new RecommendationStyleResponse { Code = "Luxury", DisplayName = "Cao cấp" }
    };

    private async Task<List<RecommendedTourResponse>> BuildCandidatesAsync(RecommendationQuery query, TravelStyleQuizResult? quiz)
    {
        var today = DateTime.UtcNow.Date;
        var tours = await _db.Tours.AsNoTracking().Where(t => EligibleTourStatuses.Contains(t.Status) && t.Capacity > 0 && t.Price > 0).ToListAsync();
        if (tours.Count == 0) return new();
        var codes = tours.Select(t => t.Code).ToList();
        var departures = await _db.GuideTourAssignments.AsNoTracking()
            .Where(d => codes.Contains(d.TourCode) && d.StartDate >= today && EligibleDepartureStatuses.Contains(d.Status))
            .OrderBy(d => d.StartDate).ToListAsync();
        var bookings = await _db.Bookings.AsNoTracking()
            .Where(b => b.TourCode != null && codes.Contains(b.TourCode) && CapacityBookingStatuses.Contains(b.Status))
            .ToListAsync();
        var tourIds = tours.Select(t => t.Id).ToList();
        var seasonPrices = await _db.TourSeasonPrices.AsNoTracking().Where(x => tourIds.Contains(x.TourId) && x.IsActive).ToListAsync();
        var promotions = await _db.TourPromotions.AsNoTracking().Where(x => tourIds.Contains(x.TourId) && x.IsActive).ToListAsync();
        var ratingRows = await (from review in _db.TourReviews.AsNoTracking()
                                join booking in _db.Bookings.AsNoTracking() on review.BookingId equals booking.Id
                                where review.Status == "Visible" && booking.TourCode != null && codes.Contains(booking.TourCode)
                                select new { booking.TourCode, review.Rating }).ToListAsync();
        var ratings = ratingRows.GroupBy(x => x.TourCode!, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => (Average: g.Average(x => x.Rating), Count: g.Count()), StringComparer.OrdinalIgnoreCase);
        var result = new List<RecommendedTourResponse>();
        foreach (var tour in tours)
        {
            if (query.ExcludeTourIds.Contains(tour.Id)) continue;
            if (!string.IsNullOrWhiteSpace(query.Destination) && !tour.Destination.Contains(query.Destination.Trim(), StringComparison.OrdinalIgnoreCase)) continue;
            if (query.DurationMin.HasValue && tour.DurationDays < query.DurationMin.Value) continue;
            if (query.DurationMax.HasValue && tour.DurationDays > query.DurationMax.Value) continue;
            var styles = ResolveStyles(tour);
            var tourType = styles.FirstOrDefault() ?? "General";
            if (!string.IsNullOrWhiteSpace(query.TourType) && !styles.Contains(query.TourType.Trim(), StringComparer.OrdinalIgnoreCase)) continue;
            var eligibleDepartures = departures.Where(d => d.TourCode.Equals(tour.Code, StringComparison.OrdinalIgnoreCase));
            if (query.StartDateFrom.HasValue) eligibleDepartures = eligibleDepartures.Where(d => d.StartDate.Date >= query.StartDateFrom.Value.Date);
            if (query.StartDateTo.HasValue) eligibleDepartures = eligibleDepartures.Where(d => d.StartDate.Date <= query.StartDateTo.Value.Date);
            var departure = eligibleDepartures.OrderBy(d => d.StartDate).FirstOrDefault();
            if (departure == null) continue;
            var booked = bookings.Where(b => b.TourCode!.Equals(tour.Code, StringComparison.OrdinalIgnoreCase) && b.DepartureDate.Date == departure.StartDate.Date).Sum(b => b.GuestCount);
            var available = Math.Max(0, tour.Capacity - booked);
            if (query.AvailableOnly && available == 0) continue;
            var price = CalculatePrice(tour, departure.StartDate.Date, seasonPrices, promotions);
            if (price.Final <= 0) continue;
            if (query.BudgetMin.HasValue && price.Final < query.BudgetMin.Value) continue;
            if (query.BudgetMax.HasValue && price.Final > query.BudgetMax.Value) continue;
            var rating = ratings.GetValueOrDefault(tour.Code);
            if (query.RatingMin.HasValue && (decimal)rating.Average < query.RatingMin.Value) continue;
            var score = 0; var reasons = new List<string>();
            if (quiz != null && styles.Contains(quiz.DominantStyle, StringComparer.OrdinalIgnoreCase)) { score += _options.PrimaryStyleWeight; reasons.Add($"Phù hợp phong cách {StyleDisplay(quiz.DominantStyle)}"); }
            if (query.BudgetMin.HasValue || query.BudgetMax.HasValue) { score += _options.BudgetWeight; reasons.Add("Trong ngân sách của bạn"); }
            if (!string.IsNullOrWhiteSpace(query.Destination)) { score += _options.DestinationWeight; reasons.Add("Đúng điểm đến mong muốn"); }
            if (!string.IsNullOrWhiteSpace(query.TourType)) { score += _options.TourTypeWeight; reasons.Add("Đúng loại tour đã chọn"); }
            if (rating.Count >= _options.MinimumReviewsForRatingScore) { var ratingScore = (int)Math.Round(rating.Average / 5d * _options.RatingMaxWeight); score += ratingScore; reasons.Add($"Đánh giá tốt {rating.Average:0.0}/5"); }
            var days = Math.Max(0, (departure.StartDate.Date - today).Days); var near = Math.Max(0, _options.NearDepartureMaxWeight - days * _options.NearDepartureMaxWeight / Math.Max(1, _options.MaximumNearDepartureDays));
            if (near > 0) { score += near; reasons.Add("Có lịch khởi hành gần"); }
            if (available <= Math.Max(2, tour.Capacity / 10)) { score -= 5; reasons.Add("Sắp hết chỗ"); }
            result.Add(new RecommendedTourResponse { TourId=tour.Id, Code=tour.Code, Name=tour.Name, Destination=tour.Destination, Region=tour.Region, TourType=tourType, TravelStyles=styles, DurationDays=tour.DurationDays, ImageUrl=tour.ImageUrl, Description=tour.Description, DepartureId=departure.Id, StartDate=departure.StartDate, EndDate=departure.EndDate, AvailableSlots=available, BasePrice=tour.Price, FinalPrice=price.Final, DiscountAmount=price.Discount, AverageRating=rating.Average, ReviewCount=rating.Count, MatchScore=score, MatchReasons=reasons });
        }
        return result;
    }

    private static (decimal Final, decimal Discount) CalculatePrice(Tour tour, DateTime date, List<TourSeasonPrice> seasons, List<TourPromotion> promotions)
    {
        var seasonal = seasons.Where(x => x.TourId == tour.Id && x.StartDate.Date <= date && x.EndDate.Date >= date).OrderByDescending(x => x.Price).FirstOrDefault();
        var beforeDiscount = seasonal?.Price ?? tour.Price;
        var promotion = promotions.Where(x => x.TourId == tour.Id && x.StartDate.Date <= date && x.EndDate.Date >= date)
            .OrderByDescending(x => Discount(beforeDiscount, x)).FirstOrDefault();
        var discount = promotion == null ? 0 : Discount(beforeDiscount, promotion);
        return (Math.Max(0, beforeDiscount - discount), discount);
    }

    private static decimal Discount(decimal price, TourPromotion p) => p.DiscountType.Equals("Percent", StringComparison.OrdinalIgnoreCase) ? Math.Min(price, price * p.DiscountValue / 100m) : Math.Min(price, p.DiscountValue);
    private static IOrderedEnumerable<RecommendedTourResponse> ApplySort(IEnumerable<RecommendedTourResponse> items, string sort) => sort.Trim().ToLowerInvariant() switch
    {
        "priceasc" => items.OrderBy(x => x.FinalPrice).ThenBy(x => x.TourId),
        "pricedesc" => items.OrderByDescending(x => x.FinalPrice).ThenBy(x => x.TourId),
        "rating" => items.OrderByDescending(x => x.AverageRating).ThenByDescending(x => x.MatchScore).ThenBy(x => x.TourId),
        "startdate" => items.OrderBy(x => x.StartDate).ThenByDescending(x => x.MatchScore).ThenBy(x => x.TourId),
        _ => items.OrderByDescending(x => x.MatchScore).ThenByDescending(x => x.AverageRating).ThenBy(x => x.StartDate).ThenBy(x => x.TourId)
    };
    private static List<string> ResolveStyles(Tour t)
    {
        var text = $"{t.Name} {t.Description} {t.Destination} {t.Region}".ToLowerInvariant(); var styles = new List<string>();
        if (ContainsAny(text,"trek","hiking","mountain","forest","safari","adventure","kayak","wildlife")) styles.Add("Adventure");
        if (ContainsAny(text,"heritage","culture","cultural","temple","museum","history","imperial","ancient","food")) styles.Add("Cultural");
        if (ContainsAny(text,"wellness","spa","beach","coastal","relax","resort","island")) styles.Add("Relaxation");
        if (ContainsAny(text,"luxury","premium","private","boutique","five-star","5 star","exclusive")) styles.Add("Luxury");
        if (styles.Count == 0) styles.Add("Cultural"); return styles;
    }
    private static bool ContainsAny(string text, params string[] keys) => keys.Any(text.Contains);
    private static string StyleDisplay(string style) => style.ToLowerInvariant() switch { "adventure"=>"Phiêu lưu", "cultural"=>"Văn hóa", "relaxation"=>"Nghỉ dưỡng", "luxury"=>"Cao cấp", _=>style };
    private static void ValidateQuery(RecommendationQuery q)
    {
        if (q.BudgetMin.HasValue && q.BudgetMax.HasValue && q.BudgetMin > q.BudgetMax) throw new ArgumentException("budgetMin must be less than or equal to budgetMax.");
        if (q.StartDateFrom.HasValue && q.StartDateTo.HasValue && q.StartDateFrom > q.StartDateTo) throw new ArgumentException("startDateFrom must be before startDateTo.");
        if (q.DurationMin.HasValue && q.DurationMax.HasValue && q.DurationMin > q.DurationMax) throw new ArgumentException("durationMin must be less than or equal to durationMax.");
        var sorts = new[] { "Recommended", "PriceAsc", "PriceDesc", "Rating", "StartDate" }; if (!sorts.Contains(q.Sort, StringComparer.OrdinalIgnoreCase)) throw new ArgumentException("Unsupported sort value.");
    }
}