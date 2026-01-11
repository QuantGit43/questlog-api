using QuestLog.Application.Interfaces;
using QuestLog.Domain.Enums;

namespace QuestLog.Infrastructure.Services;

public class AiTaskDifficultyEvaluator : ITaskDifficultyEvaluator
{
    private readonly IAiService _aiService;

    public AiTaskDifficultyEvaluator(IAiService aiService)
    {
        _aiService = aiService;
    }
    
    public async Task<DifficultyLevel> EvaluateAsync(string taskDescription)
    {
        var prompt = $@"
            Ти — помічник для управління задачами. 
            Оціни складність наступної задачі за шкалою: Easy, Medium, Hard.
            1. Easy (Легко): Задачу можна виконати менш ніж за 1 годину. Не вимагає глибокої концентрації.
            2. Medium (Середньо): Задача займає від кількох годин до 1 дня АБО вимагає спеціальних навичок.
            3. Hard (Складно): Задача займає більше кількох днів АБО вимагає значних розумових зусиль чи дослідження. 
            Задача: ""{taskDescription}""
            Відповідай тільки одним словом (Easy, Medium або Hard). Не додавай пояснень.
        ";

        try
        {
            var response = await _aiService.GetAnswerAsync(prompt);
            var cleanResponse = response.Trim().Replace(".", "");
            
            if (Enum.TryParse<DifficultyLevel>(cleanResponse, true, out var result))
            {
                return result;
            }
            
            return DifficultyLevel.Medium;
        }
        catch
        {
            return DifficultyLevel.Medium;
        }
    }
}