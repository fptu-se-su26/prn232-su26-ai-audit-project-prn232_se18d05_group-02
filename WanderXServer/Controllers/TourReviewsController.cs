using Microsoft.AspNetCore.Mvc;
using WanderXServer.Dtos.TourReviews;
using WanderXServer.Services;

namespace WanderXServer.Controllers;

[ApiController]
[Route("api/tourreviews")]
public class TourReviewsController : ControllerBase
{
    private readonly TourReviewService _service;
    private readonly IUserService _userService;

    public TourReviewsController(TourReviewService service, IUserService userService)
    {
        _service = service;
        _userService = userService;
    }

    [HttpGet("tour/{tourName}")]
    public async Task<IActionResult> GetByTourName(string tourName)
    {
        var reviews = await _service.GetByTourNameAsync(tourName);
        return Ok(reviews);
    }

    [HttpGet("average/{tourName}")]
    public async Task<IActionResult> GetAverageRating(string tourName)
    {
        var average = await _service.GetAverageRatingAsync(tourName);
        return Ok(average);
    }

    [HttpGet("booking/{bookingId:guid}")]
    public async Task<IActionResult> GetByBookingId(Guid bookingId)
    {
        var review = await _service.GetByBookingIdAsync(bookingId);
        if (review == null)
            return NoContent();
        return Ok(review);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var reviews = await _service.GetAllAsync();
        return Ok(reviews);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromQuery] string email, [FromBody] CreateTourReviewRequest request)
    {
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest(new { error = "Email is required." });

        var user = await _userService.GetUserByEmailAsync(email);
        if (user == null)
            return BadRequest(new { error = "No user found with this email." });

        try
        {
            var created = await _service.CreateAsync(request, user.Id);
            return Ok(created);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromQuery] string email, [FromBody] UpdateTourReviewRequest request)
    {
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest(new { error = "Email is required." });

        var user = await _userService.GetUserByEmailAsync(email);
        if (user == null)
            return BadRequest(new { error = "No user found with this email." });

        try
        {
            var updated = await _service.UpdateAsync(id, request, user.Id);
            return Ok(updated);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, [FromQuery] string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest(new { error = "Email is required." });

        var user = await _userService.GetUserByEmailAsync(email);
        if (user == null)
            return BadRequest(new { error = "No user found with this email." });

        try
        {
            await _service.DeleteAsync(id, user.Id);
            return Ok(new { message = "Review deleted successfully." });
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:guid}/moderate")]
    public async Task<IActionResult> Moderate(Guid id, [FromQuery] string email, [FromBody] ModerateReviewRequest request)
    {
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest(new { error = "Email is required." });

        var user = await _userService.GetUserByEmailAsync(email);
        if (user == null)
            return BadRequest(new { error = "No user found with this email." });

        try
        {
            var updated = await _service.ModerateAsync(id, request.Status, request.Reason, user.Id);
            return Ok(updated);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

public class ModerateReviewRequest
{
    public string Status { get; set; } = string.Empty;
    public string? Reason { get; set; }
}
