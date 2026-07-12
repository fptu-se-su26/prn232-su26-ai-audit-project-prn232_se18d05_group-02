using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WanderXServer.ViewModels.TourSchedules;

public class TourScheduleFormViewModel : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please choose a tour.")]
    [Display(Name = "Tour")]
    public int TourId { get; set; }

    [Range(1, 365, ErrorMessage = "Day number must be between 1 and 365.")]
    [Display(Name = "Day number")]
    public int DayNumber { get; set; } = 1;

    [Required(ErrorMessage = "Schedule date is required.")]
    [DataType(DataType.Date)]
    [Display(Name = "Schedule date")]
    public DateTime ScheduleDate { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(160, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 160 characters.")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1200, ErrorMessage = "Description must be 1200 characters or fewer.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Location is required.")]
    [StringLength(160, MinimumLength = 2, ErrorMessage = "Location must be between 2 and 160 characters.")]
    public string Location { get; set; } = string.Empty;

    [Required(ErrorMessage = "Start time is required.")]
    [DataType(DataType.Time)]
    [Display(Name = "Start time")]
    public TimeSpan StartTime { get; set; } = new(8, 0, 0);

    [Required(ErrorMessage = "End time is required.")]
    [DataType(DataType.Time)]
    [Display(Name = "End time")]
    public TimeSpan EndTime { get; set; } = new(9, 0, 0);

    [Range(1, 1000, ErrorMessage = "Sort order must be between 1 and 1000.")]
    [Display(Name = "Sort order")]
    public int SortOrder { get; set; } = 1;

    public IReadOnlyList<SelectListItem> Tours { get; set; } = Array.Empty<SelectListItem>();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndTime <= StartTime)
        {
            yield return new ValidationResult("End time must be later than start time.", new[] { nameof(EndTime) });
        }
    }
}
