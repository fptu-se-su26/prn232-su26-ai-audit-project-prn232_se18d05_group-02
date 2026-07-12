namespace WanderXServer.Dtos.Users;

public class BookingPassengerDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string TicketType { get; set; } = string.Empty;
}
