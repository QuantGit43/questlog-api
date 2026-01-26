using Microsoft.Extensions.Logging; // Додай цей namespace
using QuestLog.Application.Interfaces;
using QuestLog.Domain.Enums;

namespace QuestLog.Infrastructure.Services;

public class AiTaskDifficultyEvaluator : ITaskDifficultyEvaluator
{
    private readonly IAiService _aiService;
    private readonly ILogger<AiTaskDifficultyEvaluator> _logger;

    public AiTaskDifficultyEvaluator(IAiService aiService, ILogger<AiTaskDifficultyEvaluator> logger)
    {
        _aiService = aiService;
        _logger = logger;
    }
    
    public async Task<DifficultyLevel> EvaluateAsync(string taskDescription)
    {
        var prompt = $@"
            Analyze the complexity of this task: ""{taskDescription}""
            Return ONLY one word: Easy, Medium, or Hard.
            Do not use Markdown formatting. Do not add explanations.
        ";

        try
        {
            var response = await _aiService.GetAnswerAsync(prompt);
            
            // ЛОГУВАННЯ
            _logger.LogInformation($"[AI RAW RESPONSE]: '{response}'");

            if (string.IsNullOrWhiteSpace(response)) return DifficultyLevel.Medium;
            
            var cleanResponse = response
                .Replace("*", "")
                .Replace(".", "")
                .Replace("\"", "")
                .Trim();

            // Спроба розпарсити
            if (Enum.TryParse<DifficultyLevel>(cleanResponse, true, out var result))
            {
                return result;
            }
            
            _logger.LogWarning($"[AI PARSE ERROR]: Could not parse '{cleanResponse}' to Enum. Defaulting to Medium.");
            return DifficultyLevel.Medium;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[AI SERVICE ERROR]: Failed to evaluate task difficulty. Exception: {Message}", ex.Message);
            return DifficultyLevel.Medium;
        }
    }
}