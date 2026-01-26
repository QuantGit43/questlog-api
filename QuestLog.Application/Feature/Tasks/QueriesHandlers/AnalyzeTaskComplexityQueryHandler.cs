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
        // 1. Формуємо текст для AI (з'єднуємо заголовок і опис)
        var textToAnalyze = string.IsNullOrWhiteSpace(request.Description) 
            ? request.Title 
            : $"{request.Title}. {request.Description}";

        // 2. Викликаємо твій існуючий сервіс
        var difficulty = await _evaluator.EvaluateAsync(textToAnalyze);

        // 3. Розраховуємо нагороди (Game Balance Logic)
        // Це найкраще місце для цієї логіки, щоб вона була в одному місці
        var (xp, gold) = difficulty switch
        {
            DifficultyLevel.Easy => (10, 5),
            DifficultyLevel.Medium => (30, 15),
            DifficultyLevel.Hard => (70, 35),
            _ => (10, 5)
        };

        // 4. Повертаємо результат
        return new TaskComplexityDto
        {
            Difficulty = difficulty.ToString(),
            XpReward = xp,
            GoldReward = gold
        };
    }
}