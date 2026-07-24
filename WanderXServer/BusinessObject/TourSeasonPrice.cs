using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WanderXServer.BusinessObject;

[Index(nameof(TourId), nameof(StartDate), nameof(EndDate))]
public class TourSeasonPrice
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid TourId { get; set; }

    [Required]
    [StringLength(120)]
    public string SeasonName { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    [Range(0, 1000000)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    [ForeignKey(nameof(TourId))]
    public Tour Tour { get; set; } = null!;
}
