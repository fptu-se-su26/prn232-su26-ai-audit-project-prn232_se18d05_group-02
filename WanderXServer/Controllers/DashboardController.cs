using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Mvc; using WanderXServer.Dtos.Dashboard; using WanderXServer.Services;
namespace WanderXServer.Controllers;
[ApiController,Route("api/admin/dashboard"),Authorize(Policy="DASHBOARD_VIEW")]
public sealed class DashboardController:ControllerBase{private readonly DashboardService _s;public DashboardController(DashboardService s)=>_s=s;
 [HttpGet("summary")]public async Task<IActionResult> Summary([FromQuery]DashboardQuery q)=>await Run(()=>_s.Summary(q));
 [HttpGet("bookings-by-status")]public async Task<IActionResult> Bookings([FromQuery]DashboardQuery q)=>await Run(()=>_s.Bookings(q));
 [HttpGet("revenue-by-tour")]public async Task<IActionResult> Revenue([FromQuery]DashboardQuery q)=>await Run(()=>_s.Revenue(q));
 [HttpGet("top-guides")]public async Task<IActionResult> Guides([FromQuery]DashboardQuery q)=>await Run(()=>_s.Guides(q));
 private async Task<IActionResult> Run<T>(Func<Task<T>> action){try{return Ok(await action());}catch(ArgumentException e){return BadRequest(new ProblemDetails{Title="Invalid dashboard filter",Detail=e.Message,Status=400});}}
}