using System.ComponentModel.DataAnnotations;

namespace WanderXClient.Models;

public sealed class TourResponse
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Destination { get; set; } = string.Empty;

    public string Region { get; set; } = string.Empty;

    public int DurationDays { get; set; }

    public decimal Price { get; set; }

    public decimal OriginalPrice { get; set; }

    public decimal EffectivePrice { get; set; }

    public decimal DiscountAmount { get; set; }

    public int DiscountPercent { get; set; }

    public string? AppliedPromotionName { get; set; }

    public int Capacity { get; set; }

    public string Status { get; set; } = string.Empty;

    public string? LockReason { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

public sealed class CreateTourRequest
{
    [Required(ErrorMessage = "Tour code is required.")]
    [StringLength(32, ErrorMessage = "Tour code must be 32 characters or fewer.")]
    [RegularExpression(@"^WX-[A-Z]{2,20}-\d{3}$", ErrorMessage = "Tour code must follow WX-LOCATION-001, for example WX-HUE-226.")]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tour name is required.")]
    [StringLength(160, MinimumLength = 4, ErrorMessage = "Tour name must be between 4 and 160 characters.")]
    [RegularExpression(@"^\D+$", ErrorMessage = "Tour name cannot contain numbers.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Destination is required.")]
    [StringLength(160, MinimumLength = 2, ErrorMessage = "Destination must be between 2 and 160 characters.")]
    [RegularExpression(@"^\D+$", ErrorMessage = "Destination cannot contain numbers.")]
    public string Destination { get; set; } = string.Empty;

    [Required(ErrorMessage = "Region is required.")]
    [StringLength(160, MinimumLength = 2, ErrorMessage = "Region must be between 2 and 160 characters.")]
    public string Region { get; set; } = string.Empty;

    [Range(1, 30, ErrorMessage = "Duration must be between 1 and 30 days.")]
    public int DurationDays { get; set; } = 1;

    [Range(1, 1000000, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }

    [Range(1, 100, ErrorMessage = "Capacity must be between 1 and 100.")]
    public int Capacity { get; set; } = 1;

    [Required(ErrorMessage = "Status is required.")]
    public string Status { get; set; } = "Draft";

    [Required(ErrorMessage = "Tour image is required.")]
    [Url(ErrorMessage = "Image URL must be valid.")]
    [StringLength(500, ErrorMessage = "Image URL must be 500 characters or fewer.")]
    public string ImageUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(1200, MinimumLength = 20, ErrorMessage = "Description must be between 20 and 1200 characters.")]
    public string Description { get; set; } = string.Empty;
}

public sealed class UpdateTourRequest
{
    [Required(ErrorMessage = "Tour code is required.")]
    [StringLength(32, ErrorMessage = "Tour code must be 32 characters or fewer.")]
    [RegularExpression(@"^WX-[A-Z]{2,20}-\d{3}$", ErrorMessage = "Tour code must follow WX-LOCATION-001, for example WX-HUE-226.")]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tour name is required.")]
    [StringLength(160, MinimumLength = 4, ErrorMessage = "Tour name must be between 4 and 160 characters.")]
    [RegularExpression(@"^\D+$", ErrorMessage = "Tour name cannot contain numbers.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Destination is required.")]
    [StringLength(160, MinimumLength = 2, ErrorMessage = "Destination must be between 2 and 160 characters.")]
    [RegularExpression(@"^\D+$", ErrorMessage = "Destination cannot contain numbers.")]
    public string Destination { get; set; } = string.Empty;

    [Required(ErrorMessage = "Region is required.")]
    [StringLength(160, MinimumLength = 2, ErrorMessage = "Region must be between 2 and 160 characters.")]
    public string Region { get; set; } = string.Empty;

    [Range(1, 30, ErrorMessage = "Duration must be between 1 and 30 days.")]
    public int DurationDays { get; set; } = 1;

    [Range(1, 1000000, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }

    [Range(1, 100, ErrorMessage = "Capacity must be between 1 and 100.")]
    public int Capacity { get; set; } = 1;

    [Required(ErrorMessage = "Status is required.")]
    public string Status { get; set; } = "Draft";

    [Required(ErrorMessage = "Tour image is required.")]
    [Url(ErrorMessage = "Image URL must be valid.")]
    [StringLength(500, ErrorMessage = "Image URL must be 500 characters or fewer.")]
    public string ImageUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(1200, MinimumLength = 20, ErrorMessage = "Description must be between 20 and 1200 characters.")]
    public string Description { get; set; } = string.Empty;
}
