namespace WanderXServer.Services;

public sealed class RecommendationOptions
{
    public int PrimaryStyleWeight { get; set; } = 40;
    public int SecondaryStyleWeight { get; set; } = 20;
    public int BudgetWeight { get; set; } = 20;
    public int DestinationWeight { get; set; } = 15;
    public int TourTypeWeight { get; set; } = 10;
    public int RatingMaxWeight { get; set; } = 10;
    public int NearDepartureMaxWeight { get; set; } = 5;
    public int MinimumReviewsForRatingScore { get; set; } = 1;
    public int MaximumNearDepartureDays { get; set; } = 90;
}