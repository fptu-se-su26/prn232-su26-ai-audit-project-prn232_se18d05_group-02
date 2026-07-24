using Microsoft.EntityFrameworkCore;
using WanderXServer.BusinessObject;
using WanderXServer.DataAccessLayer;
using WanderXServer.Dtos.TourPricing;

namespace WanderXServer.Services;

public sealed class TourPricingService : ITourPricingService
{
    private static readonly string[] DiscountTypes = { "Percent", "Amount" };

    private readonly WanderXDbContext _dbContext;

    public TourPricingService(WanderXDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TourPricingResponse> GetPricingAsync(Guid tourId, DateTime? previewDate)
    {
        _dbContext.EnsureTourPricingStorage();

        var tour = await _dbContext.Tours
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == tourId);

        if (tour is null)
        {
            throw new KeyNotFoundException("Tour was not found.");
        }

        var seasonPrices = await _dbContext.TourSeasonPrices
            .AsNoTracking()
            .Where(item => item.TourId == tourId)
            .OrderByDescending(item => item.IsActive)
            .ThenBy(item => item.StartDate)
            .ToListAsync();

        var promotions = await _dbContext.TourPromotions
            .AsNoTracking()
            .Where(item => item.TourId == tourId)
            .OrderByDescending(item => item.IsActive)
            .ThenBy(item => item.StartDate)
            .ToListAsync();

        var date = (previewDate ?? DateTime.UtcNow).Date;
        var appliedSeason = seasonPrices
            .Where(item => item.IsActive && item.StartDate.Date <= date && item.EndDate.Date >= date)
            .OrderByDescending(item => item.Price)
            .FirstOrDefault();

        var priceBeforeDiscount = appliedSeason?.Price ?? tour.Price;
        var appliedPromotion = promotions
            .Where(item => item.IsActive && item.StartDate.Date <= date && item.EndDate.Date >= date)
            .OrderByDescending(item => CalculateDiscount(priceBeforeDiscount, item))
            .FirstOrDefault();

        var discountAmount = appliedPromotion is null
            ? 0
            : CalculateDiscount(priceBeforeDiscount, appliedPromotion);

        return new TourPricingResponse
        {
            TourId = tour.Id,
            TourCode = tour.Code,
            TourName = tour.Name,
            Destination = tour.Destination,
            BasePrice = tour.Price,
            PreviewDate = date,
            EffectivePrice = Math.Max(0, priceBeforeDiscount - discountAmount),
            AppliedSeasonName = appliedSeason?.SeasonName,
            AppliedPromotionName = appliedPromotion?.Name,
            DiscountAmount = discountAmount,
            SeasonPrices = seasonPrices.Select(ToSeasonResponse).ToList(),
            Promotions = promotions.Select(ToPromotionResponse).ToList()
        };
    }

    public async Task<TourSeasonPriceResponse> CreateSeasonPriceAsync(Guid tourId, UpsertTourSeasonPriceRequest request)
    {
        _dbContext.EnsureTourPricingStorage();
        await EnsureTourExistsAsync(tourId);
        ValidateSeasonPrice(request);

        var seasonPrice = new TourSeasonPrice
        {
            TourId = tourId,
            SeasonName = request.SeasonName.Trim(),
            StartDate = request.StartDate.Date,
            EndDate = request.EndDate.Date,
            Price = request.Price,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.TourSeasonPrices.Add(seasonPrice);
        await _dbContext.SaveChangesAsync();
        return ToSeasonResponse(seasonPrice);
    }

    public async Task<TourSeasonPriceResponse> UpdateSeasonPriceAsync(Guid id, UpsertTourSeasonPriceRequest request)
    {
        _dbContext.EnsureTourPricingStorage();
        ValidateSeasonPrice(request);

        var seasonPrice = await _dbContext.TourSeasonPrices.FirstOrDefaultAsync(item => item.Id == id);
        if (seasonPrice is null)
        {
            throw new KeyNotFoundException("Season price was not found.");
        }

        seasonPrice.SeasonName = request.SeasonName.Trim();
        seasonPrice.StartDate = request.StartDate.Date;
        seasonPrice.EndDate = request.EndDate.Date;
        seasonPrice.Price = request.Price;
        seasonPrice.IsActive = request.IsActive;
        seasonPrice.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        return ToSeasonResponse(seasonPrice);
    }

    public async Task DeleteSeasonPriceAsync(Guid id)
    {
        _dbContext.EnsureTourPricingStorage();
        var seasonPrice = await _dbContext.TourSeasonPrices.FirstOrDefaultAsync(item => item.Id == id);
        if (seasonPrice is null)
        {
            throw new KeyNotFoundException("Season price was not found.");
        }

        _dbContext.TourSeasonPrices.Remove(seasonPrice);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<TourPromotionResponse> CreatePromotionAsync(Guid tourId, UpsertTourPromotionRequest request)
    {
        _dbContext.EnsureTourPricingStorage();
        await EnsureTourExistsAsync(tourId);
        ValidatePromotion(request);

        var promotion = new TourPromotion
        {
            TourId = tourId,
            Name = request.Name.Trim(),
            DiscountType = NormalizeDiscountType(request.DiscountType),
            DiscountValue = request.DiscountValue,
            StartDate = request.StartDate.Date,
            EndDate = request.EndDate.Date,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.TourPromotions.Add(promotion);
        await _dbContext.SaveChangesAsync();
        return ToPromotionResponse(promotion);
    }

    public async Task<TourPromotionResponse> UpdatePromotionAsync(Guid id, UpsertTourPromotionRequest request)
    {
        _dbContext.EnsureTourPricingStorage();
        ValidatePromotion(request);

        var promotion = await _dbContext.TourPromotions.FirstOrDefaultAsync(item => item.Id == id);
        if (promotion is null)
        {
            throw new KeyNotFoundException("Promotion was not found.");
        }

        promotion.Name = request.Name.Trim();
        promotion.DiscountType = NormalizeDiscountType(request.DiscountType);
        promotion.DiscountValue = request.DiscountValue;
        promotion.StartDate = request.StartDate.Date;
        promotion.EndDate = request.EndDate.Date;
        promotion.IsActive = request.IsActive;
        promotion.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        return ToPromotionResponse(promotion);
    }

    public async Task DeletePromotionAsync(Guid id)
    {
        _dbContext.EnsureTourPricingStorage();
        var promotion = await _dbContext.TourPromotions.FirstOrDefaultAsync(item => item.Id == id);
        if (promotion is null)
        {
            throw new KeyNotFoundException("Promotion was not found.");
        }

        _dbContext.TourPromotions.Remove(promotion);
        await _dbContext.SaveChangesAsync();
    }

    private async Task EnsureTourExistsAsync(Guid tourId)
    {
        if (!await _dbContext.Tours.AnyAsync(item => item.Id == tourId))
        {
            throw new KeyNotFoundException("Tour was not found.");
        }
    }

    private static void ValidateSeasonPrice(UpsertTourSeasonPriceRequest request)
    {
        if (request.SeasonName.Trim().Length < 3)
        {
            throw new InvalidOperationException("Season name must be at least 3 characters.");
        }

        if (request.StartDate == default || request.EndDate == default)
        {
            throw new InvalidOperationException("Start date and end date are required.");
        }

        if (request.EndDate.Date < request.StartDate.Date)
        {
            throw new InvalidOperationException("End date must be after or equal to start date.");
        }

        if (request.Price <= 0)
        {
            throw new InvalidOperationException("Season price must be greater than 0.");
        }
    }

    private static void ValidatePromotion(UpsertTourPromotionRequest request)
    {
        if (request.Name.Trim().Length < 3)
        {
            throw new InvalidOperationException("Promotion name must be at least 3 characters.");
        }

        var discountType = NormalizeDiscountType(request.DiscountType);
        if (!DiscountTypes.Contains(discountType, StringComparer.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Discount type must be Percent or Amount.");
        }

        if (request.StartDate == default || request.EndDate == default)
        {
            throw new InvalidOperationException("Start date and end date are required.");
        }

        if (request.EndDate.Date < request.StartDate.Date)
        {
            throw new InvalidOperationException("End date must be after or equal to start date.");
        }

        if (discountType == "Percent" && request.DiscountValue is <= 0 or > 100)
        {
            throw new InvalidOperationException("Percent discount must be between 1 and 100.");
        }

        if (discountType == "Amount" && request.DiscountValue <= 0)
        {
            throw new InvalidOperationException("Amount discount must be greater than 0.");
        }
    }

    private static decimal CalculateDiscount(decimal price, TourPromotion promotion)
    {
        var discount = promotion.DiscountType.Equals("Percent", StringComparison.OrdinalIgnoreCase)
            ? price * promotion.DiscountValue / 100
            : promotion.DiscountValue;

        return Math.Min(price, discount);
    }

    private static string NormalizeDiscountType(string discountType)
    {
        return discountType.Trim().Equals("Amount", StringComparison.OrdinalIgnoreCase)
            ? "Amount"
            : "Percent";
    }

    private static TourSeasonPriceResponse ToSeasonResponse(TourSeasonPrice item)
    {
        return new TourSeasonPriceResponse
        {
            Id = item.Id,
            TourId = item.TourId,
            SeasonName = item.SeasonName,
            StartDate = item.StartDate,
            EndDate = item.EndDate,
            Price = item.Price,
            IsActive = item.IsActive
        };
    }

    private static TourPromotionResponse ToPromotionResponse(TourPromotion item)
    {
        return new TourPromotionResponse
        {
            Id = item.Id,
            TourId = item.TourId,
            Name = item.Name,
            DiscountType = item.DiscountType,
            DiscountValue = item.DiscountValue,
            StartDate = item.StartDate,
            EndDate = item.EndDate,
            IsActive = item.IsActive
        };
    }
}
