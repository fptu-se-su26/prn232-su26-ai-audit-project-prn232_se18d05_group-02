using System.ComponentModel.DataAnnotations;

namespace WanderXServer.Dtos.Tours;

public class UpdateTourRequest
{
    [Required]
    [StringLength(32)]
    [RegularExpression(@"^WX-[A-Z]{2,20}-\d{3}$")]
    public string Code { get; set; } = string.Empty;

    [Required]
    [StringLength(160, MinimumLength = 4)]
    [RegularExpression(@"^\D+$")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(160, MinimumLength = 2)]
    [RegularExpression(@"^\D+$")]
    public string Destination { get; set; } = string.Empty;

    [Required]
    [StringLength(160, MinimumLength = 2)]
    public string Region { get; set; } = string.Empty;

    [Range(1, 30)]
    public int DurationDays { get; set; }

    [Range(1, 1000000)]
    public decimal Price { get; set; }

    [Range(1, 100)]
    public int Capacity { get; set; }

    [Required]
    [StringLength(32)]
    public string Status { get; set; } = "Draft";

    [Required]
    [Url]
    [StringLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    [Required]
    [StringLength(1200, MinimumLength = 20)]
    public string Description { get; set; } = string.Empty;
}
