using Microsoft.EntityFrameworkCore;
using WanderXServer.BusinessObject;
using WanderXServer.DataAccessLayer;
using WanderXServer.Dtos.TourReviews;

namespace WanderXServer.Services;

public class TourReviewService
{
    private readonly WanderXDbContext _dbContext;

    public TourReviewService(WanderXDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<TourReviewResponse>> GetByTourNameAsync(string tourName)
    {
        var reviews = await _dbContext.TourReviews
            .Include(r => r.Booking)
            .Include(r => r.User)
            .Where(r => r.Booking != null && r.Booking.TourName == tourName && r.Status == "Visible")
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return reviews.Select(ToResponse).ToList();
    }

    public async Task<IReadOnlyList<TourReviewResponse>> GetAllAsync()
    {
        var reviews = await _dbContext.TourReviews
            .Include(r => r.Booking)
            .Include(r => r.User)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return reviews.Select(ToResponse).ToList();
    }

    public async Task<TourReviewResponse?> GetByBookingIdAsync(Guid bookingId)
    {
        var review = await _dbContext.TourReviews
            .Include(r => r.Booking)
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.BookingId == bookingId);

        return review is null ? null : ToResponse(review);
    }

    public async Task<TourReviewResponse> CreateAsync(CreateTourReviewRequest request, Guid userId)
    {
        var booking = await _dbContext.Bookings.FindAsync(request.BookingId);
        if (booking is null)
            throw new KeyNotFoundException("Booking not found.");

        if (booking.UserId != userId)
            throw new UnauthorizedAccessException("You can only review your own booking.");

        if (!booking.CompletedAt.HasValue)
            throw new InvalidOperationException("You can only review a tour after the trip is completed.");

        var existing = await _dbContext.TourReviews.FirstOrDefaultAsync(r => r.BookingId == request.BookingId);
        if (existing != null)
            throw new InvalidOperationException("You have already reviewed this tour. Please edit your existing review.");

        var review = new TourReview
        {
            Id = Guid.NewGuid(),
            BookingId = request.BookingId,
            UserId = userId,
            Rating = request.Rating,
            Comment = request.Comment?.Trim(),
            TravelPhotos = request.TravelPhotos,
            Status = "Visible",
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.TourReviews.Add(review);
        await _dbContext.SaveChangesAsync();

        var saved = await _dbContext.TourReviews
            .Include(r => r.Booking)
            .Include(r => r.User)
            .FirstAsync(r => r.Id == review.Id);

        return ToResponse(saved);
    }

    public async Task<TourReviewResponse> UpdateAsync(Guid id, UpdateTourReviewRequest request, Guid userId)
    {
        var review = await _dbContext.TourReviews
            .Include(r => r.Booking)
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (review is null)
            throw new KeyNotFoundException("Review not found.");

        if (review.UserId != userId)
            throw new UnauthorizedAccessException("You can only edit your own review.");

        review.Rating = request.Rating;
        review.Comment = request.Comment?.Trim();
        review.TravelPhotos = request.TravelPhotos;
        review.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return ToResponse(review);
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        var review = await _dbContext.TourReviews.FindAsync(id);
        if (review is null)
            throw new KeyNotFoundException("Review not found.");

        if (review.UserId != userId)
            throw new UnauthorizedAccessException("You can only delete your own review.");

        _dbContext.TourReviews.Remove(review);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<TourReviewResponse> ModerateAsync(Guid id, string status, string? reason, Guid moderatorId)
    {
        var review = await _dbContext.TourReviews
            .Include(r => r.Booking)
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (review is null)
            throw new KeyNotFoundException("Review not found.");

        var allowedStatuses = new[] { "Visible", "Hidden", "Deleted" };
        if (!allowedStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
            throw new InvalidOperationException("Invalid status. Accepted values: Visible, Hidden, Deleted.");

        review.Status = status.Trim();
        review.ModerationReason = reason?.Trim();
        review.ModeratedAt = DateTime.UtcNow;
        review.ModeratedBy = moderatorId;

        await _dbContext.SaveChangesAsync();

        return ToResponse(review);
    }

    public async Task<double> GetAverageRatingAsync(string tourName)
    {
        var ratings = await _dbContext.TourReviews
            .Include(r => r.Booking)
            .Where(r => r.Booking != null && r.Booking.TourName == tourName && r.Status == "Visible")
            .Select(r => r.Rating)
            .ToListAsync();

        return ratings.Count == 0 ? 0.0 : ratings.Average();
    }

    private static TourReviewResponse ToResponse(TourReview review)
    {
        return new TourReviewResponse
        {
            Id = review.Id,
            BookingId = review.BookingId,
            BookingCode = review.Booking?.BookingCode ?? string.Empty,
            TourName = review.Booking?.TourName ?? string.Empty,
            UserId = review.UserId,
            UserName = review.User?.FullName ?? string.Empty,
            UserAvatar = review.User?.Address ?? string.Empty,
            Rating = review.Rating,
            Comment = review.Comment,
            TravelPhotos = review.TravelPhotos,
            Status = review.Status,
            CreatedAt = review.CreatedAt,
            UpdatedAt = review.UpdatedAt,
            ModeratedAt = review.ModeratedAt,
            ModerationReason = review.ModerationReason,
            ModeratedBy = review.ModeratedBy,
            ModeratorName = string.Empty
        };
    }
}
