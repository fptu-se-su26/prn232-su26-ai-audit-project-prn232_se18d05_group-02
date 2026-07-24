using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WanderXServer.Dtos.Users;
using WanderXServer.Services;

namespace WanderXServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TravelStyleQuizController : ControllerBase
{
    private readonly TravelStyleQuizService _quizService;

    public TravelStyleQuizController(TravelStyleQuizService quizService)
    {
        _quizService = quizService;
    }

    [Authorize(Roles = "Customer")]
    [HttpGet("history")]
    public async Task<ActionResult<IEnumerable<TravelStyleQuizResultResponse>>> GetHistory()
    {
        if (!TryGetCurrentUserId(out var userId)) return Unauthorized();

        try
        {
            return Ok(await _quizService.GetHistoryByUserIdAsync(userId));
        }
        catch (InvalidOperationException exception)
        {
            return NotFound(CreateProblem("User not found", exception.Message, StatusCodes.Status404NotFound));
        }
    }

    [Authorize(Roles = "Customer")]
    [HttpGet("latest")]
    public async Task<ActionResult<TravelStyleQuizResultResponse>> GetLatest()
    {
        if (!TryGetCurrentUserId(out var userId)) return Unauthorized();

        try
        {
            var latest = await _quizService.GetLatestByUserIdAsync(userId);
            return latest == null ? NoContent() : Ok(latest);
        }
        catch (InvalidOperationException exception)
        {
            return NotFound(CreateProblem("User not found", exception.Message, StatusCodes.Status404NotFound));
        }
    }

    [Authorize(Roles = "Customer")]
    [HttpPost]
    public async Task<ActionResult<TravelStyleQuizResultResponse>> SubmitQuiz(
        [FromBody] TravelStyleQuizSubmitRequest request)
    {
        if (!TryGetCurrentUserId(out var userId)) return Unauthorized();

        try
        {
            return Ok(await _quizService.SubmitQuizAsync(userId, request));
        }
        catch (ArgumentException exception)
        {
            return BadRequest(CreateProblem("Invalid quiz submission", exception.Message, StatusCodes.Status400BadRequest));
        }
        catch (InvalidOperationException exception)
        {
            return NotFound(CreateProblem("User not found", exception.Message, StatusCodes.Status404NotFound));
        }
    }

    [AllowAnonymous]
    [HttpGet("questions")]
    public async Task<ActionResult<IEnumerable<QuizQuestionDto>>> GetQuestions()
    {
        return Ok(await _quizService.GetQuestionsAsync());
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("questions")]
    public async Task<ActionResult<QuizQuestionDto>> CreateQuestion([FromBody] QuizQuestionDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return Ok(await _quizService.CreateQuestionAsync(dto));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("questions/{id:guid}")]
    public async Task<ActionResult<QuizQuestionDto>> UpdateQuestion(Guid id, [FromBody] QuizQuestionDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        try
        {
            return Ok(await _quizService.UpdateQuestionAsync(id, dto));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(CreateProblem("Question not found", $"No question with ID {id} was found.", StatusCodes.Status404NotFound));
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("questions/{id:guid}")]
    public async Task<IActionResult> DeleteQuestion(Guid id)
    {
        try
        {
            await _quizService.DeleteQuestionAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(CreateProblem("Question not found", $"No question with ID {id} was found.", StatusCodes.Status404NotFound));
        }
    }

    private bool TryGetCurrentUserId(out Guid userId)
    {
        return Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
    }

    private static ProblemDetails CreateProblem(string title, string detail, int status)
    {
        return new ProblemDetails { Title = title, Detail = detail, Status = status };
    }
}