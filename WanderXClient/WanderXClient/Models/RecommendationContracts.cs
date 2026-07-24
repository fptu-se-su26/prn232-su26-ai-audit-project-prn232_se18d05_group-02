namespace WanderXClient.Models;

public sealed class RecommendationPageResponse
{
    public bool HasQuizResult { get; set; }
    public string? PrimaryStyle { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public string? Message { get; set; }
    public List<RecommendedTourResponse> Items { get; set; } = new();
}

public sealed class RecommendedTourResponse
{
    public Guid TourId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string TourType { get; set; } = string.Empty;
    public List<string> TravelStyles { get; set; } = new();
    public int DurationDays { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid DepartureId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int AvailableSlots { get; set; }
    public decimal BasePrice { get; set; }
    public decimal FinalPrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public string Currency { get; set; } = "VND";
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public int MatchScore { get; set; }
    public List<string> MatchReasons { get; set; } = new();
}

public sealed class RecommendationFilter
{
    public decimal? BudgetMin { get; set; }
    public decimal? BudgetMax { get; set; }
    public string Destination { get; set; } = string.Empty;
    public string TourType { get; set; } = string.Empty;
    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo { get; set; }
    public int? DurationMin { get; set; }
    public int? DurationMax { get; set; }
    public decimal? RatingMin { get; set; }
    public string Sort { get; set; } = "Recommended";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 9;
}