using Microsoft.AspNetCore.Mvc;
using WanderXServer.Services;
using WanderXServer.ViewModels.TourSchedules;

namespace WanderXServer.Controllers;

[Route("admin/tour-schedules")]
public class AdminTourSchedulesController : Controller
{
    private readonly ITourScheduleService _tourScheduleService;

    public AdminTourSchedulesController(ITourScheduleService tourScheduleService)
    {
        _tourScheduleService = tourScheduleService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index([FromQuery] TourScheduleFilterViewModel filter)
    {
        return View(await _tourScheduleService.GetIndexAsync(filter));
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create([FromQuery] int? tourId)
    {
        return View(await _tourScheduleService.CreateFormAsync(tourId));
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TourScheduleFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Tours = await _tourScheduleService.GetTourOptionsAsync(model.TourId);
            return View(model);
        }

        try
        {
            var id = await _tourScheduleService.CreateAsync(model);
            TempData["SuccessMessage"] = "Tour schedule created.";
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (KeyNotFoundException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            model.Tours = await _tourScheduleService.GetTourOptionsAsync(model.TourId);
            return View(model);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            return View(await _tourScheduleService.GetDetailsAsync(id));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            return View(await _tourScheduleService.EditFormAsync(id));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TourScheduleFormViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            model.Tours = await _tourScheduleService.GetTourOptionsAsync(model.TourId);
            return View(model);
        }

        try
        {
            await _tourScheduleService.UpdateAsync(model);
            TempData["SuccessMessage"] = "Tour schedule updated.";
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }
        catch (KeyNotFoundException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            model.Tours = await _tourScheduleService.GetTourOptionsAsync(model.TourId);
            return View(model);
        }
    }

    [HttpGet("{id:int}/delete")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            return View(await _tourScheduleService.GetDetailsAsync(id));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _tourScheduleService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Tour schedule deleted.";
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("{id:int}/move-up")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MoveUp(int id)
    {
        await _tourScheduleService.MoveUpAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/move-down")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MoveDown(int id)
    {
        await _tourScheduleService.MoveDownAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
