using System.ComponentModel.DataAnnotations;

namespace WanderXServer.Dtos.TourPricing;

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

    public string DiscountType { get; set; } = string.Empty;

    public decimal DiscountValue { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }
}

public sealed class UpsertTourSeasonPriceRequest
{
    [Required]
    [StringLength(120, MinimumLength = 3)]
    public string SeasonName { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    [Range(1, 1000000)]
    public decimal Price { get; set; }

    public bool IsActive { get; set; } = true;
}

public sealed class UpsertTourPromotionRequest
{
    [Required]
    [StringLength(120, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(16)]
    public string DiscountType { get; set; } = "Percent";

    [Range(1, 1000000)]
    public decimal DiscountValue { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; } = true;
}
