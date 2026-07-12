using Microsoft.AspNetCore.Mvc.Rendering;
using WanderXServer.ViewModels.TourSchedules;

namespace WanderXServer.Services;

public interface ITourScheduleService
{
    Task<TourScheduleIndexViewModel> GetIndexAsync(TourScheduleFilterViewModel filter);

    Task<TourScheduleDetailsViewModel> GetDetailsAsync(int id);

    Task<TourScheduleFormViewModel> CreateFormAsync(int? tourId = null);

    Task<int> CreateAsync(TourScheduleFormViewModel model);

    Task<TourScheduleFormViewModel> EditFormAsync(int id);

    Task UpdateAsync(TourScheduleFormViewModel model);

    Task DeleteAsync(int id);

    Task MoveUpAsync(int id);

    Task MoveDownAsync(int id);

    Task<IReadOnlyList<SelectListItem>> GetTourOptionsAsync(int? selectedTourId = null);
}
