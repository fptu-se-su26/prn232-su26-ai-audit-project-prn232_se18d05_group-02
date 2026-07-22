using Microsoft.AspNetCore.Mvc;
using WanderXServer.Dtos.Bookings;
using WanderXServer.Services;

namespace WanderXServer.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BookingResponse>>> GetAll([FromQuery] string? status, [FromQuery] string? search)
    {
        return Ok(await _bookingService.GetAllAsync(status, search));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BookingResponse>> GetById(Guid id)
    {
        try
        {
            return Ok(await _bookingService.GetByIdAsync(id));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Booking not found", exception.Message, StatusCodes.Status404NotFound));
        }
    }

    [HttpGet("cancellations")]
    public async Task<ActionResult<IReadOnlyList<BookingResponse>>> GetCancellationRequests(
        [FromQuery] string? status,
        [FromQuery] string? search)
    {
        try
        {
            return Ok(await _bookingService.GetCancellationRequestsAsync(status, search));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(ToProblem("Cancellation request query failed", exception.Message, StatusCodes.Status400BadRequest));
        }
    }

    [HttpGet("payments")]
    public async Task<ActionResult<IReadOnlyList<BookingResponse>>> GetPayments(
        [FromQuery] string? paymentStatus,
        [FromQuery] string? search)
    {
        try
        {
            return Ok(await _bookingService.GetPaymentsAsync(paymentStatus, search));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(ToProblem("Payment query failed", exception.Message, StatusCodes.Status400BadRequest));
        }
    }

    [HttpPost]
    public async Task<ActionResult<BookingResponse>> Create(CreateBookingRequest request)
    {
        try
        {
            var booking = await _bookingService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Booking creation failed", exception.Message, StatusCodes.Status404NotFound));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(ToProblem("Booking creation failed", exception.Message, StatusCodes.Status400BadRequest));
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BookingResponse>> Update(Guid id, UpdateBookingRequest request)
    {
        try
        {
            return Ok(await _bookingService.UpdateAsync(id, request));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Booking update failed", exception.Message, StatusCodes.Status404NotFound));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(ToProblem("Booking update failed", exception.Message, StatusCodes.Status400BadRequest));
        }
    }

    [HttpPut("{id:guid}/status")]
    public async Task<ActionResult<BookingResponse>> UpdateStatus(Guid id, UpdateBookingStatusRequest request)
    {
        try
        {
            return Ok(await _bookingService.UpdateStatusAsync(id, request));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Booking status update failed", exception.Message, StatusCodes.Status404NotFound));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(ToProblem("Booking status update failed", exception.Message, StatusCodes.Status400BadRequest));
        }
    }

    [HttpPut("{id:guid}/cancellation/review")]
    public async Task<ActionResult<BookingResponse>> ReviewCancellation(Guid id, ReviewCancellationRequest request)
    {
        try
        {
            return Ok(await _bookingService.ReviewCancellationRequestAsync(id, request));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Cancellation review failed", exception.Message, StatusCodes.Status404NotFound));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(ToProblem("Cancellation review failed", exception.Message, StatusCodes.Status400BadRequest));
        }
    }

    [HttpPut("{id:guid}/payment")]
    public async Task<ActionResult<BookingResponse>> UpdatePayment(Guid id, UpdatePaymentRequest request)
    {
        try
        {
            return Ok(await _bookingService.UpdatePaymentAsync(id, request));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Payment update failed", exception.Message, StatusCodes.Status404NotFound));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(ToProblem("Payment update failed", exception.Message, StatusCodes.Status400BadRequest));
        }
    }

    private static ProblemDetails ToProblem(string title, string detail, int status)
    {
        return new ProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = status
        };
    }
}
