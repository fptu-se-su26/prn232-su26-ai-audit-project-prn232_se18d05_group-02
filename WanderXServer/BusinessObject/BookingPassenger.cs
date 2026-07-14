using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WanderXServer.BusinessObject;

public class BookingPassenger
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid BookingId { get; set; }

    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string TicketType { get; set; } = "Người lớn";

    [ForeignKey(nameof(BookingId))]
    public Booking? Booking { get; set; }
}
