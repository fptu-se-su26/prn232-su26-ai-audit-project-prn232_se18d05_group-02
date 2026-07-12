using Microsoft.AspNetCore.Mvc;
using WanderXServer.Dtos.UserSpecialRequests;
using WanderXServer.Services;

namespace WanderXServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserSpecialRequestsController : ControllerBase
{
    private readonly UserSpecialRequestService _service;

    public UserSpecialRequestsController(UserSpecialRequestService service)
    {
        _service = service;
    }

    [HttpGet("booking/{bookingId}")]
    public async Task<ActionResult<IReadOnlyList<UserSpecialRequestResponse>>> GetByBookingId(Guid bookingId)
    {
        var requests = await _service.GetByBookingIdAsync(bookingId);
        return Ok(requests);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IReadOnlyList<UserSpecialRequestResponse>>> GetByUserId(Guid userId)
    {
        var requests = await _service.GetByUserIdAsync(userId);
        return Ok(requests);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserSpecialRequestResponse>> GetById(Guid id)
    {
        var request = await _service.GetByIdAsync(id);
        return Ok(request);
    }

    [HttpPost]
    public async Task<ActionResult<UserSpecialRequestResponse>> Create([FromBody] CreateUserSpecialRequestRequest request)
    {
        var created = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserSpecialRequestResponse>> Update(Guid id, [FromBody] UpdateUserSpecialRequestRequest request)
    {
        var updated = await _service.UpdateAsync(id, request);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
