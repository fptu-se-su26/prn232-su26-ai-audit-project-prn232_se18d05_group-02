using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WanderXServer.BusinessObject;
using WanderXServer.DataAccessLayer;
using WanderXServer.Dtos.Users;

namespace WanderXServer.Services;

public class TravelStyleQuizService
{
    private readonly WanderXDbContext _dbContext;

    public TravelStyleQuizService(WanderXDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<TravelStyleQuizResultResponse>> GetHistoryByUserIdAsync(Guid userId)
    {
        if (!await _dbContext.Users.AnyAsync(u => u.Id == userId))
        {
            throw new InvalidOperationException("User not found.");
        }

        var results = await _dbContext.TravelStyleQuizResults
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CompletedAt)
            .ToListAsync();

        return results.Select(ToResponseDto).ToList();
    }

    public async Task<TravelStyleQuizResultResponse?> GetLatestByUserIdAsync(Guid userId)
    {
        if (!await _dbContext.Users.AnyAsync(u => u.Id == userId))
        {
            throw new InvalidOperationException("User not found.");
        }

        var result = await _dbContext.TravelStyleQuizResults
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CompletedAt)
            .FirstOrDefaultAsync();

        if (result == null)
        {
            return null;
        }

        return ToResponseDto(result);
    }

    public async Task<TravelStyleQuizResultResponse> SubmitQuizAsync(
        Guid userId,
        TravelStyleQuizSubmitRequest request)
    {
        if (!await _dbContext.Users.AnyAsync(u => u.Id == userId))
        {
            throw new InvalidOperationException("User not found.");
        }

        var dbQuestions = await _dbContext.QuizQuestions
            .Include(q => q.Options)
            .OrderBy(q => q.DisplayOrder)
            .ToListAsync();

        if (dbQuestions.Count == 0)
        {
            throw new ArgumentException("QUIZ_NOT_AVAILABLE: There is no active quiz.");
        }

        if (request.Answers == null)
        {
            throw new ArgumentException("QUESTION_REQUIRED: Answers are required.");
        }

        var duplicateQuestionIds = request.Answers
            .GroupBy(answer => answer.QuestionId)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToList();
        if (duplicateQuestionIds.Count > 0)
        {
            throw new ArgumentException(
                $"INVALID_OPTION: Duplicate answers for question(s): {string.Join(", ", duplicateQuestionIds)}.");
        }

        var requiredQuestionIds = dbQuestions.Select(question => question.Id).ToHashSet();
        var submittedQuestionIds = request.Answers.Select(answer => answer.QuestionId).ToHashSet();
        var missingQuestionIds = requiredQuestionIds.Except(submittedQuestionIds).ToList();
        if (missingQuestionIds.Count > 0)
        {
            throw new ArgumentException(
                $"QUESTION_REQUIRED: Missing answer(s) for question(s): {string.Join(", ", missingQuestionIds)}.");
        }

        var unknownQuestionIds = submittedQuestionIds.Except(requiredQuestionIds).ToList();
        if (unknownQuestionIds.Count > 0)
        {
            throw new ArgumentException(
                $"INVALID_OPTION: Unknown question(s): {string.Join(", ", unknownQuestionIds)}.");
        }

        var adventureScore = 0;
        var culturalScore = 0;
        var relaxationScore = 0;
        var luxuryScore = 0;

        foreach (var answer in request.Answers)
        {
            var dbQuestion = dbQuestions.Single(question => question.Id == answer.QuestionId);
            var dbOption = dbQuestion.Options.SingleOrDefault(option => option.Id == answer.OptionId);
            if (dbOption == null)
            {
                throw new ArgumentException(
                    $"INVALID_OPTION: Option {answer.OptionId} does not belong to question {answer.QuestionId}.");
            }

            var category = dbOption.Category.Trim().ToLowerInvariant();
            var score = dbOption.Score;
            if (category == "adventure") adventureScore += score;
            else if (category == "cultural") culturalScore += score;
            else if (category == "relaxation") relaxationScore += score;
            else if (category == "luxury") luxuryScore += score;
            else
            {
                throw new ArgumentException(
                    $"INVALID_OPTION: Unsupported travel style category '{dbOption.Category}'.");
            }
        }

        var maxScore = Math.Max(adventureScore, Math.Max(culturalScore, Math.Max(relaxationScore, luxuryScore)));
        var dominantStyle = "Cultural";
        if (maxScore == adventureScore) dominantStyle = "Adventure";
        else if (maxScore == culturalScore) dominantStyle = "Cultural";
        else if (maxScore == relaxationScore) dominantStyle = "Relaxation";
        else if (maxScore == luxuryScore) dominantStyle = "Luxury";

        var quizResult = new TravelStyleQuizResult
        {
            UserId = userId,
            AnswersJson = JsonSerializer.Serialize(request.Answers),
            AdventureScore = adventureScore,
            CulturalScore = culturalScore,
            RelaxationScore = relaxationScore,
            LuxuryScore = luxuryScore,
            DominantStyle = dominantStyle,
            CompletedAt = DateTime.UtcNow
        };

        _dbContext.TravelStyleQuizResults.Add(quizResult);
        await _dbContext.SaveChangesAsync();

        return ToResponseDto(quizResult);
    }
    // --- Quiz Questions CRUD for Admin ---

    public async Task<IReadOnlyList<QuizQuestionDto>> GetQuestionsAsync()
    {
        var questions = await _dbContext.QuizQuestions
            .Include(q => q.Options)
            .OrderBy(q => q.DisplayOrder)
            .ToListAsync();

        return questions.Select(q => new QuizQuestionDto
        {
            Id = q.Id,
            Text = q.Text,
            DisplayOrder = q.DisplayOrder,
            Options = q.Options.Select(o => new QuizOptionDto
            {
                Id = o.Id,
                QuestionId = o.QuestionId,
                OptionKey = o.OptionKey,
                Text = o.Text,
                Category = o.Category,
                Score = o.Score
            }).OrderBy(o => o.OptionKey).ToList()
        }).ToList();
    }

    public async Task<QuizQuestionDto> CreateQuestionAsync(QuizQuestionDto dto)
    {
        var maxOrder = await _dbContext.QuizQuestions.AnyAsync()
            ? await _dbContext.QuizQuestions.MaxAsync(q => q.DisplayOrder)
            : 0;

        var question = new QuizQuestion
        {
            Text = dto.Text.Trim(),
            DisplayOrder = maxOrder + 1
        };

        foreach (var opt in dto.Options)
        {
            question.Options.Add(new QuizOption
            {
                OptionKey = opt.OptionKey.Trim().ToUpperInvariant(),
                Text = opt.Text.Trim(),
                Category = opt.Category.Trim(),
                Score = opt.Score > 0 ? opt.Score : 3
            });
        }

        _dbContext.QuizQuestions.Add(question);
        await _dbContext.SaveChangesAsync();

        dto.Id = question.Id;
        return dto;
    }

    public async Task<QuizQuestionDto> UpdateQuestionAsync(Guid id, QuizQuestionDto dto)
    {
        var question = await _dbContext.QuizQuestions
            .Include(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (question == null)
        {
            throw new KeyNotFoundException("Quiz question not found.");
        }

        question.Text = dto.Text.Trim();
        if (dto.DisplayOrder > 0)
        {
            question.DisplayOrder = dto.DisplayOrder;
        }

        // Remove old options and add new ones to avoid complex sync
        _dbContext.QuizOptions.RemoveRange(question.Options);
        question.Options.Clear();

        foreach (var opt in dto.Options)
        {
            question.Options.Add(new QuizOption
            {
                OptionKey = opt.OptionKey.Trim().ToUpperInvariant(),
                Text = opt.Text.Trim(),
                Category = opt.Category.Trim(),
                Score = opt.Score > 0 ? opt.Score : 3
            });
        }

        await _dbContext.SaveChangesAsync();
        return dto;
    }

    public async Task DeleteQuestionAsync(Guid id)
    {
        var question = await _dbContext.QuizQuestions.FindAsync(id);
        if (question == null)
        {
            throw new KeyNotFoundException("Quiz question not found.");
        }

        _dbContext.QuizQuestions.Remove(question);
        await _dbContext.SaveChangesAsync();

        // Re-order remaining questions
        var remaining = await _dbContext.QuizQuestions
            .OrderBy(q => q.DisplayOrder)
            .ToListAsync();

        for (int i = 0; i < remaining.Count; i++)
        {
            remaining[i].DisplayOrder = i + 1;
        }
        await _dbContext.SaveChangesAsync();
    }

    private static TravelStyleQuizResultResponse ToResponseDto(TravelStyleQuizResult result)
    {
        return new TravelStyleQuizResultResponse
        {
            Id = result.Id,
            UserId = result.UserId,
            AnswersJson = result.AnswersJson,
            AdventureScore = result.AdventureScore,
            CulturalScore = result.CulturalScore,
            RelaxationScore = result.RelaxationScore,
            LuxuryScore = result.LuxuryScore,
            DominantStyle = result.DominantStyle,
            CompletedAt = result.CompletedAt
        };
    }
}
