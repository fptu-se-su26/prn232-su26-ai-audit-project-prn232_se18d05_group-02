using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using WanderXServer.Dtos.Tours;
using WanderXServer.Services;

namespace WanderXServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToursController : ControllerBase
{
    private readonly ITourService _tourService;

    public ToursController(ITourService tourService)
    {
        _tourService = tourService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TourResponse>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] string? status,
        [FromQuery] string? destination,
        [FromQuery] DateTime? departureDate)
    {
        try
        {
            return Ok(await _tourService.GetAllAsync(search, status, destination, departureDate));
        }
        catch (SqlException exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ToProblem("Database unavailable", ToDatabaseMessage(exception), StatusCodes.Status503ServiceUnavailable));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TourResponse>> GetById(Guid id)
    {
        try
        {
            return Ok(await _tourService.GetByIdAsync(id));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Tour not found", exception.Message, StatusCodes.Status404NotFound));
        }
        catch (SqlException exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ToProblem("Database unavailable", ToDatabaseMessage(exception), StatusCodes.Status503ServiceUnavailable));
        }
    }

    [HttpPost]
    public async Task<ActionResult<TourResponse>> Create(CreateTourRequest request)
    {
        try
        {
            var tour = await _tourService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = tour.Id }, tour);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(ToProblem("Tour creation failed", exception.Message, StatusCodes.Status400BadRequest));
        }
        catch (SqlException exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ToProblem("Database unavailable", ToDatabaseMessage(exception), StatusCodes.Status503ServiceUnavailable));
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TourResponse>> Update(Guid id, UpdateTourRequest request)
    {
        try
        {
            return Ok(await _tourService.UpdateAsync(id, request));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Tour not found", exception.Message, StatusCodes.Status404NotFound));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(ToProblem("Tour update failed", exception.Message, StatusCodes.Status400BadRequest));
        }
        catch (SqlException exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ToProblem("Database unavailable", ToDatabaseMessage(exception), StatusCodes.Status503ServiceUnavailable));
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _tourService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Tour not found", exception.Message, StatusCodes.Status404NotFound));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(ToProblem("Tour delete failed", exception.Message, StatusCodes.Status400BadRequest));
        }
        catch (SqlException exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ToProblem("Database unavailable", ToDatabaseMessage(exception), StatusCodes.Status503ServiceUnavailable));
        }
    }

    [HttpPatch("{id:guid}/hide")]
    public async Task<ActionResult<TourResponse>> Hide(Guid id)
    {
        try
        {
            return Ok(await _tourService.HideAsync(id));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Tour not found", exception.Message, StatusCodes.Status404NotFound));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(ToProblem("Tour hide failed", exception.Message, StatusCodes.Status400BadRequest));
        }
        catch (SqlException exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ToProblem("Database unavailable", ToDatabaseMessage(exception), StatusCodes.Status503ServiceUnavailable));
        }
    }

    [HttpPatch("{id:guid}/lock")]
    public async Task<ActionResult<TourResponse>> Lock(Guid id)
    {
        try
        {
            return Ok(await _tourService.LockAsync(id));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Tour not found", exception.Message, StatusCodes.Status404NotFound));
        }
        catch (SqlException exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ToProblem("Database unavailable", ToDatabaseMessage(exception), StatusCodes.Status503ServiceUnavailable));
        }
    }

    [HttpPatch("{id:guid}/unlock")]
    public async Task<ActionResult<TourResponse>> Unlock(Guid id)
    {
        try
        {
            return Ok(await _tourService.UnlockAsync(id));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Tour not found", exception.Message, StatusCodes.Status404NotFound));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(ToProblem("Tour unlock failed", exception.Message, StatusCodes.Status400BadRequest));
        }
        catch (SqlException exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ToProblem("Database unavailable", ToDatabaseMessage(exception), StatusCodes.Status503ServiceUnavailable));
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

    private static string ToDatabaseMessage(SqlException exception)
    {
        return $"The database could not be reached. Please start SQL Server SQLEXPRESS and try again. Detail: {exception.Message}";
    }
}
