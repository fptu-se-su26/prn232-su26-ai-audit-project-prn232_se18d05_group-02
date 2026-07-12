using System.ComponentModel.DataAnnotations;

namespace WanderXClient.Models;

public sealed class UserProfileResponse// TV3
{ 
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public sealed class UpdateProfileRequest
{
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(120, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 120 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Enter a valid phone number.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [StringLength(240)]
    public string? Address { get; set; }

    public string? CurrentPassword { get; set; }
    
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    public string? NewPassword { get; set; }
}

public sealed class BookingSummaryResponse
{
    public Guid Id { get; set; }
    public string BookingCode { get; set; } = string.Empty;
    public string TourName { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public DateTime DepartureDate { get; set; }
    public int GuestCount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    // Tiến trình đơn hàng (null = chưa xử lý)
    public DateTime? PaidAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    // Approval và Failure states
    public DateTime? ApprovedAt { get; set; }
    public DateTime? RequestFailedAt { get; set; }
    public DateTime? PaymentFailedAt { get; set; }
    public DateTime? ConfirmationFailedAt { get; set; }
    public DateTime? CompletionFailedAt { get; set; }

    public List<BookingPassengerDto> Passengers { get; set; } = new();
}

public class BookingPassengerDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string TicketType { get; set; } = string.Empty;
}

// end TV3
