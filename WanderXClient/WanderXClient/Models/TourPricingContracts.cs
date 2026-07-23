using System.ComponentModel.DataAnnotations;

namespace WanderXClient.Models;

public sealed class TourPricingResponse
{
    public Guid TourId { get; set; }

    public string TourCode { get; set; } = string.Empty;

    public string TourName { get; set; } = string.Empty;

    public string Destination { get; set; } = string.Empty;

    public decimal BasePrice { get; set; }

    public DateTime PreviewDate { get; set; }

    public decimal EffectivePrice { get; set; }

    public string? AppliedSeasonName { get; set; }

    public string? AppliedPromotionName { get; set; }

    public decimal DiscountAmount { get; set; }

    public List<TourSeasonPriceResponse> SeasonPrices { get; set; } = new();

    public List<TourPromotionResponse> Promotions { get; set; } = new();
}

public sealed class TourSeasonPriceResponse
{
    public Guid Id { get; set; }

    public Guid TourId { get; set; }

    public string SeasonName { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal Price { get; set; }

    public bool IsActive { get; set; }
}

public sealed class TourPromotionResponse
{
    public Guid Id { get; set; }

    public Guid TourId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string DiscountType { get; set; } = "Percent";

    public decimal DiscountValue { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }
}

public sealed class UpsertTourSeasonPriceRequest
{
    [Required(ErrorMessage = "Season name is required.")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "Season name must be between 3 and 120 characters.")]
    public string SeasonName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Start date is required.")]
    public DateTime StartDate { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "End date is required.")]
    public DateTime EndDate { get; set; } = DateTime.Today.AddDays(7);

    [Range(1, 1000000, ErrorMessage = "Season price must be greater than 0.")]
    public decimal Price { get; set; }

    public bool IsActive { get; set; } = true;
}

public sealed class UpsertTourPromotionRequest
{
    [Required(ErrorMessage = "Promotion name is required.")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "Promotion name must be between 3 and 120 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Discount type is required.")]
    public string DiscountType { get; set; } = "Percent";

    [Range(1, 1000000, ErrorMessage = "Discount value must be greater than 0.")]
    public decimal DiscountValue { get; set; }

    [Required(ErrorMessage = "Start date is required.")]
    public DateTime StartDate { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "End date is required.")]
    public DateTime EndDate { get; set; } = DateTime.Today.AddDays(7);

    public bool IsActive { get; set; } = true;
}
