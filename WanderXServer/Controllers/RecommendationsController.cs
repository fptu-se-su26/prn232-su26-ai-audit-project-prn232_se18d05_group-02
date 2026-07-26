using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WanderXServer.Dtos.Recommendations;
using WanderXServer.Services;

namespace WanderXServer.Controllers;

[ApiController]
[Route("api/recommendations")]
public sealed class RecommendationsController : ControllerBase
{
    private readonly RecommendationService _service;
    public RecommendationsController(RecommendationService service) => _service = service;

    [Authorize(Roles = "Customer")]
    [HttpGet("me")]
    public async Task<ActionResult<RecommendationPageResponse>> GetMine([FromQuery] RecommendationQuery query)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        try { return Ok(await _service.GetForUserAsync(userId, query)); }
        catch (ArgumentException ex) { return BadRequest(Problem("Invalid recommendation filter", ex.Message, 400)); }
    }

    [AllowAnonymous]
    [HttpGet("/api/tours/search")]
    public async Task<ActionResult<RecommendationPageResponse>> Search([FromQuery] RecommendationQuery query)
    {
        try { return Ok(await _service.GetForUserAsync(GetOptionalUserId(), query)); }
        catch (ArgumentException ex) { return BadRequest(Problem("Invalid tour filter", ex.Message, 400)); }
    }

    [AllowAnonymous]
    [HttpGet("/api/tours/random")]
    public async Task<ActionResult<RecommendedTourResponse>> Random([FromQuery] RecommendationQuery query)
    {
        try
        {
            var item = await _service.GetRandomAsync(GetOptionalUserId(), query);
            return item == null ? NoContent() : Ok(item);
        }
        catch (ArgumentException ex) { return BadRequest(Problem("Invalid tour filter", ex.Message, 400)); }
    }

    [AllowAnonymous]
    [HttpGet("/api/recommendation-config/styles")]
    public ActionResult<IReadOnlyList<RecommendationStyleResponse>> GetStyles() => Ok(RecommendationService.GetStyles());

    private Guid? GetOptionalUserId() => TryGetUserId(out var id) ? id : null;
    private bool TryGetUserId(out Guid id) => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out id);
    private static ProblemDetails Problem(string title, string detail, int status) => new() { Title = title, Detail = detail, Status = status };
}