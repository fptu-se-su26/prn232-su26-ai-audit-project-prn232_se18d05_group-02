using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WanderXServer.BusinessObject;

[Index(nameof(TourId), nameof(StartDate), nameof(EndDate))]
public class TourPromotion
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid TourId { get; set; }

    [Required]
    [StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(16)]
    public string DiscountType { get; set; } = "Percent";

    [Range(0, 1000000)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountValue { get; set; }

    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    [ForeignKey(nameof(TourId))]
    public Tour Tour { get; set; } = null!;
}
