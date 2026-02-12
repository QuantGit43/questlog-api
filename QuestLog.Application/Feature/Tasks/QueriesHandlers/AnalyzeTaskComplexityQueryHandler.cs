using MediatR;
using QuestLog.Application.Dto;
using QuestLog.Application.Feature.Tasks.Queries; 
using QuestLog.Application.Interfaces; 
using QuestLog.Domain.Enums;

namespace QuestLog.Application.Feature.Tasks.QueriesHandlers;

public class AnalyzeTaskComplexityHandler : IRequestHandler<AnalyzeTaskComplexityQuery, TaskComplexityDto>
{
    private readonly ITaskAnalyser _taskAnalyzer;

    public AnalyzeTaskComplexityHandler(ITaskAnalyser taskAnalyzer)
    {
        _taskAnalyzer = taskAnalyzer;
    }

    public async Task<TaskComplexityDto> Handle(AnalyzeTaskComplexityQuery request, CancellationToken cancellationToken)
    {
        var textToAnalyze = string.IsNullOrWhiteSpace(request.Description) 
            ? request.Title 
            : $"{request.Title}. {request.Description}";

        var analysisResult = await _taskAnalyzer.AnalyseAsync(textToAnalyze);

        var (xp, gold) = CalculateRewards(analysisResult.Difficulty);

        var calculatedDueDate = CalculateDueDate(analysisResult.Difficulty);

        return new TaskComplexityDto
        {
            Difficulty = analysisResult.Difficulty.ToString(),
            Category = analysisResult.Category.ToString(), 
            DueDate = calculatedDueDate,                   
            XpReward = xp,
            GoldReward = gold
        };
    }

    private (int xp, int gold) CalculateRewards(DifficultyLevel difficulty)
    {
        return difficulty switch
        {
            DifficultyLevel.Easy => (10, 5),
            DifficultyLevel.Medium => (30, 15),
            DifficultyLevel.Hard => (70, 35),
            _ => (10, 5)
        };
    }

    private DateTime CalculateDueDate(DifficultyLevel difficulty)
    {
        var random = new Random();
        var now = DateTime.UtcNow;

        return difficulty switch
        {
            DifficultyLevel.Easy => now.AddHours(random.Next(12, 49)),   // 12-48 годин
            DifficultyLevel.Medium => now.AddDays(random.Next(3, 8)),    // 3-7 днів
            DifficultyLevel.Hard => now.AddDays(random.Next(14, 31)),    // 2-4 тижні
            _ => now.AddDays(1)
        };
    }
}