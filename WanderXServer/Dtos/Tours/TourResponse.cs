namespace WanderXServer.Dtos.Tours;

public class TourResponse
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
