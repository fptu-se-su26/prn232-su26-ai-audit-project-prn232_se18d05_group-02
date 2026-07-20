using Microsoft.AspNetCore.Mvc;
using WanderXServer.Dtos.TourSchedules;
using WanderXServer.Services;
using WanderXServer.ViewModels.TourSchedules;

namespace WanderXServer.Controllers;

[ApiController]
[Route("api/tour-schedules")]
public class TourSchedulesController : ControllerBase
{
    private readonly ITourScheduleService _tourScheduleService;

    public TourSchedulesController(ITourScheduleService tourScheduleService)
    {
        _tourScheduleService = tourScheduleService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TourScheduleResponse>>> GetAll([FromQuery] TourScheduleFilterRequest request)
    {
        var model = await _tourScheduleService.GetIndexAsync(new TourScheduleFilterViewModel
        {
            TourId = request.TourId,
            FilterDate = request.FilterDate,
            Month = request.Month,
            Year = request.Year
        });

        return Ok(model.Schedules.Select(row => new TourScheduleResponse
        {
            Id = row.Id,
            TourId = row.TourId,
            TourName = row.TourName,
            DayNumber = row.DayNumber,
            ScheduleDate = row.ScheduleDate,
            DayOfWeek = row.DayOfWeek,
            Title = row.Title,
            Location = row.Location,
            StartTime = row.StartTime,
            EndTime = row.EndTime,
            SortOrder = row.SortOrder
        }).ToList());
    }

    [HttpGet("tour-options")]
    public async Task<ActionResult<IReadOnlyList<TourScheduleTourOptionResponse>>> GetTourOptions()
    {
        var options = await _tourScheduleService.GetTourOptionsAsync();

        return Ok(options.Select(option => new TourScheduleTourOptionResponse
        {
            TourId = int.Parse(option.Value),
            Name = option.Text
        }).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TourScheduleResponse>> GetById(int id)
    {
        try
        {
            return Ok(ToResponse(await _tourScheduleService.GetDetailsAsync(id)));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Tour schedule not found", exception.Message, StatusCodes.Status404NotFound));
        }
    }

    [HttpPost]
    public async Task<ActionResult<TourScheduleResponse>> Create(CreateTourScheduleRequest request)
    {
        try
        {
            var id = await _tourScheduleService.CreateAsync(ToForm(request));
            return CreatedAtAction(nameof(GetById), new { id }, ToResponse(await _tourScheduleService.GetDetailsAsync(id)));
        }
        catch (KeyNotFoundException exception)
        {
            return BadRequest(ToProblem("Tour schedule creation failed", exception.Message, StatusCodes.Status400BadRequest));
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TourScheduleResponse>> Update(int id, UpdateTourScheduleRequest request)
    {
        try
        {
            var form = ToForm(request);
            form.Id = id;
            await _tourScheduleService.UpdateAsync(form);
            return Ok(ToResponse(await _tourScheduleService.GetDetailsAsync(id)));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Tour schedule update failed", exception.Message, StatusCodes.Status404NotFound));
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _tourScheduleService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Tour schedule not found", exception.Message, StatusCodes.Status404NotFound));
        }
    }

    [HttpPost("{id:int}/move-up")]
    public async Task<IActionResult> MoveUp(int id)
    {
        await _tourScheduleService.MoveUpAsync(id);
        return NoContent();
    }

    [HttpPost("{id:int}/move-down")]
    public async Task<IActionResult> MoveDown(int id)
    {
        await _tourScheduleService.MoveDownAsync(id);
        return NoContent();
    }

    private static TourScheduleFormViewModel ToForm(CreateTourScheduleRequest request)
    {
        return new TourScheduleFormViewModel
        {
            TourId = request.TourId,
            DayNumber = request.DayNumber,
            ScheduleDate = request.ScheduleDate,
            Title = request.Title,
            Description = request.Description,
            Location = request.Location,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            SortOrder = request.SortOrder
        };
    }

    private static TourScheduleResponse ToResponse(TourScheduleDetailsViewModel model)
    {
        return new TourScheduleResponse
        {
            Id = model.Id,
            TourId = model.TourId,
            TourName = model.TourName,
            DayNumber = model.DayNumber,
            ScheduleDate = model.ScheduleDate,
            DayOfWeek = model.DayOfWeek,
            Title = model.Title,
            Description = model.Description,
            Location = model.Location,
            StartTime = model.StartTime,
            EndTime = model.EndTime,
            SortOrder = model.SortOrder
        };
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
