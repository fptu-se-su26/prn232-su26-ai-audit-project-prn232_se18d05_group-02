using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WanderXServer.Dtos.Users;
using WanderXServer.Dtos.UserSpecialRequests;
using WanderXServer.Services;

namespace WanderXServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly UserSpecialRequestService _specialRequestService;

    public UsersController(IUserService userService, UserSpecialRequestService specialRequestService)
    {
        _userService = userService;
        _specialRequestService = specialRequestService;
    }

    [HttpGet("profile")]
    public async Task<ActionResult<UserProfileResponse>> GetProfile([FromQuery] string email)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Invalid request",
                    Detail = "Email is required.",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            return Ok(await _userService.GetProfileAsync(email));
        }
        catch (InvalidOperationException exception)
        {
            return NotFound(new ProblemDetails
            {
                Title = "User not found",
                Detail = exception.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
    }

    [HttpPut("profile")]
    public async Task<ActionResult<UserProfileResponse>> UpdateProfile([FromQuery] string email, [FromBody] UpdateProfileRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Invalid request",
                    Detail = "Email is required.",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            return Ok(await _userService.UpdateProfileAsync(email, request));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Update failed",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    [HttpGet("bookings")]
    public async Task<ActionResult<IEnumerable<BookingSummaryResponse>>> GetBookings([FromQuery] string email)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Invalid request",
                    Detail = "Email is required.",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            return Ok(await _userService.GetBookingsAsync(email));
        }
        catch (InvalidOperationException exception)
        {
            return NotFound(new ProblemDetails
            {
                Title = "User not found",
                Detail = exception.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
    }
    // TV3
    [HttpGet("bookings/{id:guid}")]
    public async Task<ActionResult<BookingSummaryResponse>> GetBookingById(Guid id)
    {
        var booking = await _userService.GetBookingByIdAsync(id);
        if (booking == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Booking not found",
                Detail = $"No booking found with id {id}.",
                Status = StatusCodes.Status404NotFound
            });
        }
        return Ok(booking);
    }

    [HttpPut("bookings/{id:guid}/cancel")]
    public async Task<ActionResult<BookingSummaryResponse>> CancelBooking(Guid id)
    {
        try
        {
            var booking = await _userService.CancelBookingAsync(id);
            if (booking == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Booking not found",
                    Detail = $"No booking found with id {id}.",
                    Status = StatusCodes.Status404NotFound
                });
            }
            return Ok(booking);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Cancellation failed",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    [HttpGet("special-requests")]
    public async Task<ActionResult<IEnumerable<UserSpecialRequestResponse>>> GetSpecialRequests([FromQuery] string email)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Invalid request",
                    Detail = "Email is required.",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "User not found",
                    Detail = $"No user found with email {email}.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            var requests = await _specialRequestService.GetByUserIdAsync(user.Id);
            return Ok(requests);
        }
        catch (InvalidOperationException exception)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Error",
                Detail = exception.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
    }

    [HttpPost("special-requests")]
    public async Task<ActionResult<UserSpecialRequestResponse>> CreateSpecialRequest([FromBody] CreateUserSpecialRequestRequest request)
    {
        try
        {
            var created = await _specialRequestService.CreateAsync(request);
            return Ok(created);
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Booking not found",
                Detail = exception.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Creation failed",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    // TV3 - Admin endpoints for special requests management
    [HttpGet("admin/bookings-with-requests")]
    public async Task<ActionResult<IEnumerable<BookingWithRequestCountResponse>>> GetAllBookingsWithRequestCounts()
    {
        try
        {
            var result = await _specialRequestService.GetAllBookingsWithRequestCountsAsync();
            return Ok(result);
        }
        catch (Exception exception)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Failed to load bookings",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    [HttpGet("admin/bookings/{bookingId:guid}/special-requests")]
    public async Task<ActionResult<IEnumerable<UserSpecialRequestResponse>>> GetSpecialRequestsByBooking(Guid bookingId)
    {
        try
        {
            var requests = await _specialRequestService.GetByBookingIdAsync(bookingId);
            return Ok(requests);
        }
        catch (Exception exception)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Failed to load requests",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    [HttpPut("admin/special-requests/{id:guid}/review")]
    public async Task<ActionResult<UserSpecialRequestResponse>> ReviewSpecialRequest(Guid id, [FromBody] UpdateUserSpecialRequestRequest request)
    {
        try
        {
            var updated = await _specialRequestService.UpdateAsync(id, request);
            return Ok(updated);
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Request not found",
                Detail = exception.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Review failed",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }
    // end TV3
}
