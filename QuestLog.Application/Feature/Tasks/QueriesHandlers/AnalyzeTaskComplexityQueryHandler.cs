using MediatR;
using QuestLog.Application.Dto;
using QuestLog.Application.Feature.Tasks.Queries;
using QuestLog.Application.Interfaces; // Тут лежить інтерфейс ITaskDifficultyEvaluator
using QuestLog.Domain.Enums;

namespace QuestLog.Application.Feature.Tasks.QueriesHandlers;

public class AnalyzeTaskComplexityHandler : IRequestHandler<AnalyzeTaskComplexityQuery, TaskComplexityDto>
{
    private readonly ITaskDifficultyEvaluator _evaluator;

    public AnalyzeTaskComplexityHandler(ITaskDifficultyEvaluator evaluator)
    {
        _evaluator = evaluator;
    }

    public async Task<TaskComplexityDto> Handle(AnalyzeTaskComplexityQuery request, CancellationToken cancellationToken)
    {
        var textToAnalyze = string.IsNullOrWhiteSpace(request.Description) 
            ? request.Title 
            : $"{request.Title}. {request.Description}";

        var difficulty = await _evaluator.EvaluateAsync(textToAnalyze);

        var (xp, gold) = difficulty switch
        {
            DifficultyLevel.Easy => (10, 5),
            DifficultyLevel.Medium => (30, 15),
            DifficultyLevel.Hard => (70, 35),
            _ => (10, 5)
        };

        return new TaskComplexityDto
        {
            Difficulty = difficulty.ToString(),
            XpReward = xp,
            GoldReward = gold
        };
    }
}