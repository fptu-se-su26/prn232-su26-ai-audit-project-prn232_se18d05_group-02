using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WanderXServer.BusinessObject;

[Index(nameof(Code), IsUnique = true)]
public class Tour
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public int ScheduleTourId { get; set; }

    [Required]
    [StringLength(32)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [StringLength(160)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(160)]
    public string Destination { get; set; } = string.Empty;

    [Required]
    [StringLength(160)]
    public string Region { get; set; } = string.Empty;

    [Range(1, 365)]
    public int DurationDays { get; set; }

    [Range(0, 1000000)]
    public decimal Price { get; set; }

    [Range(1, 1000)]
    public int Capacity { get; set; }

    [Required]
    [StringLength(32)]
    public string Status { get; set; } = "Draft";

    [StringLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    [StringLength(1200)]
    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public ICollection<TourSchedule> Schedules { get; set; } = new List<TourSchedule>();
}
