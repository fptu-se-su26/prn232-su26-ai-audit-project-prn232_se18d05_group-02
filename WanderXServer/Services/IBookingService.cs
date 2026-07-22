using WanderXServer.Dtos.Bookings;

namespace WanderXServer.Services;

public interface IBookingService
{
    Task<IReadOnlyList<BookingResponse>> GetAllAsync(string? status, string? search);
    Task<BookingResponse> GetByIdAsync(Guid id);
    Task<BookingResponse> CreateAsync(CreateBookingRequest request);
    Task<BookingResponse> UpdateAsync(Guid id, UpdateBookingRequest request);
    Task<BookingResponse> UpdateStatusAsync(Guid id, UpdateBookingStatusRequest request);
    Task<IReadOnlyList<BookingResponse>> GetCancellationRequestsAsync(string? status, string? search);
    Task<BookingResponse> ReviewCancellationRequestAsync(Guid id, ReviewCancellationRequest request);
    Task<IReadOnlyList<BookingResponse>> GetPaymentsAsync(string? paymentStatus, string? search);
    Task<BookingResponse> UpdatePaymentAsync(Guid id, UpdatePaymentRequest request);
}
