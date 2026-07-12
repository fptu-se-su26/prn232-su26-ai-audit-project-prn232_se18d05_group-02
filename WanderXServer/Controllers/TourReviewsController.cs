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

    // GET api/tourreviews/tour/{tourName}
    [HttpGet("tour/{tourName}")]
    public async Task<IActionResult> GetByTourName(string tourName)
    {
        var reviews = await _service.GetByTourNameAsync(tourName);
        return Ok(reviews);
    }

    // GET api/tourreviews/average/{tourName}
    [HttpGet("average/{tourName}")]
    public async Task<IActionResult> GetAverageRating(string tourName)
    {
        var average = await _service.GetAverageRatingAsync(tourName);
        return Ok(average);
    }

    // GET api/tourreviews/booking/{bookingId}
    [HttpGet("booking/{bookingId:guid}")]
    public async Task<IActionResult> GetByBookingId(Guid bookingId)
    {
        var review = await _service.GetByBookingIdAsync(bookingId);
        if (review == null)
            return NoContent(); // 204 khi chưa có review
        return Ok(review);
    }

    // GET api/tourreviews (admin)
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var reviews = await _service.GetAllAsync();
        return Ok(reviews);
    }

    // POST api/tourreviews?email=...
    [HttpPost]
    public async Task<IActionResult> Create([FromQuery] string email, [FromBody] CreateTourReviewRequest request)
    {
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest(new { error = "Email không được để trống." });

        var user = await _userService.GetUserByEmailAsync(email);
        if (user == null)
            return BadRequest(new { error = "Không tìm thấy người dùng với email này." });

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

    // PUT api/tourreviews/{id}?email=...
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromQuery] string email, [FromBody] UpdateTourReviewRequest request)
    {
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest(new { error = "Email không được để trống." });

        var user = await _userService.GetUserByEmailAsync(email);
        if (user == null)
            return BadRequest(new { error = "Không tìm thấy người dùng với email này." });

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

    // DELETE api/tourreviews/{id}?email=...
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, [FromQuery] string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest(new { error = "Email không được để trống." });

        var user = await _userService.GetUserByEmailAsync(email);
        if (user == null)
            return BadRequest(new { error = "Không tìm thấy người dùng với email này." });

        try
        {
            await _service.DeleteAsync(id, user.Id);
            return Ok(new { message = "Xóa đánh giá thành công." });
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

    // PUT api/tourreviews/{id}/moderate?email=...
    [HttpPut("{id:guid}/moderate")]
    public async Task<IActionResult> Moderate(Guid id, [FromQuery] string email, [FromBody] ModerateReviewRequest request)
    {
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest(new { error = "Email không được để trống." });

        var user = await _userService.GetUserByEmailAsync(email);
        if (user == null)
            return BadRequest(new { error = "Không tìm thấy người dùng với email này." });

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
