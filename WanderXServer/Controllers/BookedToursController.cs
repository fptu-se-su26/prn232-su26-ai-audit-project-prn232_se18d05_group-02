using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WanderXServer.Dtos.BookedTours;
using WanderXServer.Services;

namespace WanderXServer.Controllers;

[ApiController]
[Route("api/booked-tours")]
public sealed class BookedToursController : ControllerBase
{
    private readonly IBookedTourService _bookedTourService;

    public BookedToursController(IBookedTourService bookedTourService)
    {
        _bookedTourService = bookedTourService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BookedTourResponse>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] string? status,
        [FromQuery] string? destination,
        [FromQuery] DateTime? departureDate)
    {
        try
        {
            var filter = new BookedTourFilterRequest
            {
                Search = search,
                Status = status,
                Destination = destination,
                DepartureDate = departureDate
            };

            return Ok(await _bookedTourService.GetBookedToursAsync(filter));
        }
        catch (SqlException exception)
        {
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                new ProblemDetails
                {
                    Title = "Database unavailable",
                    Detail = $"The database could not be reached. Detail: {exception.Message}",
                    Status = StatusCodes.Status503ServiceUnavailable
                });
        }
    }
}
