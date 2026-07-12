using System.ComponentModel.DataAnnotations;

namespace WanderXServer.Dtos.TourReviews;

public class UpdateTourReviewRequest
{
    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    [StringLength(2000)]
    public string? Comment { get; set; }

    public string? TravelPhotos { get; set; }
}
