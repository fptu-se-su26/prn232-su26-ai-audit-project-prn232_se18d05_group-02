using Microsoft.AspNetCore.Mvc;
using WanderXServer.Dtos.GuideTours;
using WanderXServer.Services;

namespace WanderXServer.Controllers;

[ApiController]
[Route("api/guide-tours")]
public class GuideToursController : ControllerBase
{
    private readonly IGuideTourService _guideTourService;

    public GuideToursController(IGuideTourService guideTourService)
    {
        _guideTourService = guideTourService;
    }

    [HttpGet("schedule")]
    public async Task<ActionResult<IReadOnlyList<GuideTourAssignmentResponse>>> GetSchedule([FromQuery] string email)
    {
        try
        {
            return Ok(await _guideTourService.GetScheduleAsync(email));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Guide not found", exception.Message, StatusCodes.Status404NotFound));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GuideTourAssignmentResponse>> GetById(Guid id)
    {
        try
        {
            return Ok(await _guideTourService.GetByIdAsync(id));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Tour assignment not found", exception.Message, StatusCodes.Status404NotFound));
        }
    }

    [HttpPost("{id:guid}/decline")]
    public async Task<ActionResult<GuideTourAssignmentResponse>> Decline(Guid id, DeclineTourRequest request)
    {
        try
        {
            return Ok(await _guideTourService.DeclineAsync(id, request));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Tour assignment not found", exception.Message, StatusCodes.Status404NotFound));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(ToProblem("Tour decline failed", exception.Message, StatusCodes.Status400BadRequest));
        }
    }

    [HttpPut("{id:guid}/finish")]
    public async Task<ActionResult<GuideTourAssignmentResponse>> Finish(Guid id)
    {
        try
        {
            return Ok(await _guideTourService.FinishAsync(id));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Tour assignment not found", exception.Message, StatusCodes.Status404NotFound));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(ToProblem("Tour status update failed", exception.Message, StatusCodes.Status400BadRequest));
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
