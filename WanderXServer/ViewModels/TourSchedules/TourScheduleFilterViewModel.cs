using Microsoft.AspNetCore.Mvc.Rendering;

namespace WanderXServer.ViewModels.TourSchedules;

public class TourScheduleFilterViewModel
{
    public int? TourId { get; set; }

    public DateTime? FilterDate { get; set; }

    public int? Month { get; set; }

    public int? Year { get; set; }

    public IReadOnlyList<SelectListItem> Tours { get; set; } = Array.Empty<SelectListItem>();
}
