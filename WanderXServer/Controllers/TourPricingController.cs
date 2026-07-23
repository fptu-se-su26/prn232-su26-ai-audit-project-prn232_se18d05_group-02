using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WanderXServer.Dtos.TourPricing;
using WanderXServer.Services;

namespace WanderXServer.Controllers;

[ApiController]
[Route("api/tour-pricing")]
public sealed class TourPricingController : ControllerBase
{
    private readonly ITourPricingService _tourPricingService;

    public TourPricingController(ITourPricingService tourPricingService)
    {
        _tourPricingService = tourPricingService;
    }

    [HttpGet("{tourId:guid}")]
    public async Task<ActionResult<TourPricingResponse>> GetPricing(Guid tourId, [FromQuery] DateTime? previewDate)
    {
        try
        {
            return Ok(await _tourPricingService.GetPricingAsync(tourId, previewDate));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Tour pricing not found", exception.Message, StatusCodes.Status404NotFound));
        }
        catch (SqlException exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ToProblem("Database unavailable", exception.Message, StatusCodes.Status503ServiceUnavailable));
        }
    }

    [HttpPost("{tourId:guid}/season-prices")]
    public async Task<ActionResult<TourSeasonPriceResponse>> CreateSeasonPrice(Guid tourId, UpsertTourSeasonPriceRequest request)
    {
        try
        {
            return Ok(await _tourPricingService.CreateSeasonPriceAsync(tourId, request));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Tour not found", exception.Message, StatusCodes.Status404NotFound));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(ToProblem("Season price validation failed", exception.Message, StatusCodes.Status400BadRequest));
        }
        catch (SqlException exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ToProblem("Database unavailable", exception.Message, StatusCodes.Status503ServiceUnavailable));
        }
    }

    [HttpPut("season-prices/{id:guid}")]
    public async Task<ActionResult<TourSeasonPriceResponse>> UpdateSeasonPrice(Guid id, UpsertTourSeasonPriceRequest request)
    {
        try
        {
            return Ok(await _tourPricingService.UpdateSeasonPriceAsync(id, request));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Season price not found", exception.Message, StatusCodes.Status404NotFound));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(ToProblem("Season price validation failed", exception.Message, StatusCodes.Status400BadRequest));
        }
        catch (SqlException exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ToProblem("Database unavailable", exception.Message, StatusCodes.Status503ServiceUnavailable));
        }
    }

    [HttpDelete("season-prices/{id:guid}")]
    public async Task<IActionResult> DeleteSeasonPrice(Guid id)
    {
        try
        {
            await _tourPricingService.DeleteSeasonPriceAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Season price not found", exception.Message, StatusCodes.Status404NotFound));
        }
    }

    [HttpPost("{tourId:guid}/promotions")]
    public async Task<ActionResult<TourPromotionResponse>> CreatePromotion(Guid tourId, UpsertTourPromotionRequest request)
    {
        try
        {
            return Ok(await _tourPricingService.CreatePromotionAsync(tourId, request));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Tour not found", exception.Message, StatusCodes.Status404NotFound));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(ToProblem("Promotion validation failed", exception.Message, StatusCodes.Status400BadRequest));
        }
        catch (SqlException exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ToProblem("Database unavailable", exception.Message, StatusCodes.Status503ServiceUnavailable));
        }
    }

    [HttpPut("promotions/{id:guid}")]
    public async Task<ActionResult<TourPromotionResponse>> UpdatePromotion(Guid id, UpsertTourPromotionRequest request)
    {
        try
        {
            return Ok(await _tourPricingService.UpdatePromotionAsync(id, request));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Promotion not found", exception.Message, StatusCodes.Status404NotFound));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(ToProblem("Promotion validation failed", exception.Message, StatusCodes.Status400BadRequest));
        }
        catch (SqlException exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ToProblem("Database unavailable", exception.Message, StatusCodes.Status503ServiceUnavailable));
        }
    }

    [HttpDelete("promotions/{id:guid}")]
    public async Task<IActionResult> DeletePromotion(Guid id)
    {
        try
        {
            await _tourPricingService.DeletePromotionAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(ToProblem("Promotion not found", exception.Message, StatusCodes.Status404NotFound));
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
