namespace WanderXClient.Models;

public sealed class TourReviewResponse
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public string BookingCode { get; set; } = string.Empty;
    public string TourName { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserAvatar { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public string? TravelPhotos { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ModeratedAt { get; set; }
    public string? ModerationReason { get; set; }
    public Guid? ModeratedBy { get; set; }
    public string? ModeratorName { get; set; } = string.Empty;
}

public sealed class CreateTourReviewRequest
{
    public Guid BookingId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public string? TravelPhotos { get; set; }
}

public sealed class UpdateTourReviewRequest
{
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public string? TravelPhotos { get; set; }
}

public sealed class ModerateReviewRequest
{
    public string Status { get; set; } = string.Empty;
    public string? Reason { get; set; }
}
