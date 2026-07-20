namespace WanderXServer.ViewModels.TourSchedules;

public class TourScheduleRowViewModel
{
    public int Id { get; set; }

    public int TourId { get; set; }

    public string TourName { get; set; } = string.Empty;

    public int DayNumber { get; set; }

    public DateTime ScheduleDate { get; set; }

    public string DayOfWeek => ScheduleDate.ToString("dddd");

    public string Title { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public int SortOrder { get; set; }
}
