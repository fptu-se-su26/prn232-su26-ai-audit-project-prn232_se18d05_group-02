using WanderXServer.Dtos.TourPricing;

namespace WanderXServer.Services;

public interface ITourPricingService
{
    Task<TourPricingResponse> GetPricingAsync(Guid tourId, DateTime? previewDate);

    Task<TourSeasonPriceResponse> CreateSeasonPriceAsync(Guid tourId, UpsertTourSeasonPriceRequest request);

    Task<TourSeasonPriceResponse> UpdateSeasonPriceAsync(Guid id, UpsertTourSeasonPriceRequest request);

    Task DeleteSeasonPriceAsync(Guid id);

    Task<TourPromotionResponse> CreatePromotionAsync(Guid tourId, UpsertTourPromotionRequest request);

    Task<TourPromotionResponse> UpdatePromotionAsync(Guid id, UpsertTourPromotionRequest request);

    Task DeletePromotionAsync(Guid id);
}
