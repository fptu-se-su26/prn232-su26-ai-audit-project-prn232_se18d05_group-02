using System.ComponentModel.DataAnnotations;

namespace WanderXServer.Dtos.TourSchedules;

public sealed class TourScheduleFilterRequest
{
    public int? TourId { get; set; }

    public DateTime? FilterDate { get; set; }

    public int? Month { get; set; }

    public int? Year { get; set; }
}

public sealed class TourScheduleResponse
{
    public int Id { get; set; }

    public int TourId { get; set; }

    public string TourName { get; set; } = string.Empty;

    public int DayNumber { get; set; }

    public DateTime ScheduleDate { get; set; }

    public string DayOfWeek { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public int SortOrder { get; set; }
}

public sealed class TourScheduleTourOptionResponse
{
    public int TourId { get; set; }

    public string Name { get; set; } = string.Empty;
}

public class CreateTourScheduleRequest : IValidatableObject
{
    [Range(1, int.MaxValue, ErrorMessage = "Please choose a tour.")]
    public int TourId { get; set; }

    [Range(1, 365, ErrorMessage = "Day number must be between 1 and 365.")]
    public int DayNumber { get; set; } = 1;

    [Required(ErrorMessage = "Schedule date is required.")]
    public DateTime ScheduleDate { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(160, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 160 characters.")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1200, ErrorMessage = "Description must be 1200 characters or fewer.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Location is required.")]
    [StringLength(160, MinimumLength = 2, ErrorMessage = "Location must be between 2 and 160 characters.")]
    public string Location { get; set; } = string.Empty;

    public TimeSpan StartTime { get; set; } = new(8, 0, 0);

    public TimeSpan EndTime { get; set; } = new(9, 0, 0);

    [Range(1, 1000, ErrorMessage = "Sort order must be between 1 and 1000.")]
    public int SortOrder { get; set; } = 1;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndTime <= StartTime)
        {
            yield return new ValidationResult("End time must be later than start time.", new[] { nameof(EndTime) });
        }
    }
}

public sealed class UpdateTourScheduleRequest : CreateTourScheduleRequest
{
}
