using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WanderXServer.BusinessObject;

[Index(nameof(TourId), nameof(ScheduleDate), nameof(DayNumber), nameof(SortOrder))]
public class TourSchedule
{
    [Key]
    public int Id { get; set; }

    public int TourId { get; set; }

    [Range(1, 365)]
    public int DayNumber { get; set; }

    [DataType(DataType.Date)]
    public DateTime ScheduleDate { get; set; }

    [Required]
    [StringLength(160)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1200)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [StringLength(160)]
    public string Location { get; set; } = string.Empty;

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    [Range(1, 1000)]
    public int SortOrder { get; set; }

    [ForeignKey(nameof(TourId))]
    public Tour Tour { get; set; } = null!;
}
