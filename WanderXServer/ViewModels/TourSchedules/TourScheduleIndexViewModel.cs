namespace WanderXServer.ViewModels.TourSchedules;

public class TourScheduleIndexViewModel
{
    public TourScheduleFilterViewModel Filter { get; set; } = new();

    public IReadOnlyList<TourScheduleRowViewModel> Schedules { get; set; } = Array.Empty<TourScheduleRowViewModel>();
}
