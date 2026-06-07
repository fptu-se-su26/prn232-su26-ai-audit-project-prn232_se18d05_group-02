using Microsoft.AspNetCore.Mvc;
using WanderXServer.Dtos.Guides;
using WanderXServer.Services;

namespace WanderXServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GuidesController : ControllerBase
{
    private readonly IGuideService _guideService;

    public GuidesController(IGuideService guideService)
    {
        _guideService = guideService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GuideResponse>>> GetAll([FromQuery] string? search)
    {
        return Ok(await _guideService.GetAllAsync(search));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GuideResponse>> GetById(Guid id)
    {
        try
        {
            return Ok(await _guideService.GetByIdAsync(id));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Guide not found", exception.Message, StatusCodes.Status404NotFound));
        }
    }

    [HttpGet("by-email")]
    public async Task<ActionResult<GuideResponse>> GetByEmail([FromQuery] string email)
    {
        try
        {
            return Ok(await _guideService.GetByEmailAsync(email));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Guide not found", exception.Message, StatusCodes.Status404NotFound));
        }
    }

    [HttpGet("email-exists")]
    public async Task<ActionResult<object>> EmailExists([FromQuery] string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest(ToProblem("Email is required", "Email is required.", StatusCodes.Status400BadRequest));
        }

        return Ok(new { exists = await _guideService.EmailExistsAsync(email) });
    }

    [HttpPost]
    public async Task<ActionResult<GuideResponse>> Create(CreateGuideRequest request)
    {
        try
        {
            var guide = await _guideService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = guide.Id }, guide);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(ToProblem("Guide creation failed", exception.Message, StatusCodes.Status400BadRequest));
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<GuideResponse>> Update(Guid id, UpdateGuideRequest request)
    {
        try
        {
            return Ok(await _guideService.UpdateAsync(id, request));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Guide not found", exception.Message, StatusCodes.Status404NotFound));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(ToProblem("Guide update failed", exception.Message, StatusCodes.Status400BadRequest));
        }
    }

    [HttpPut("self-profile")]
    public async Task<ActionResult<GuideResponse>> SelfUpdate(GuideSelfUpdateRequest request)
    {
        try
        {
            return Ok(await _guideService.SelfUpdateAsync(request));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Guide not found", exception.Message, StatusCodes.Status404NotFound));
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
