using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using QuestLog.Application.Dto;
using QuestLog.Application.Interfaces;
using QuestLog.Domain.Enums;

namespace QuestLog.Infrastructure.Services;

public class AiTaskAnalyzer : ITaskAnalyser
{
    private readonly IAiService _aiService;
    private readonly ILogger<AiTaskAnalyzer> _logger;

    public AiTaskAnalyzer(IAiService aiService, ILogger<AiTaskAnalyzer> logger)
    {
        _aiService = aiService;
        _logger = logger;
    }

    public async Task<AiAnalysisResult> AnalyseAsync(string taskDescription)
    {
        // Отримуємо список доступних категорій, щоб AI вибирав тільки з них
        var categories = string.Join(", ", Enum.GetNames(typeof(TaskCategory)));
        var difficulties = string.Join(", ", Enum.GetNames(typeof(DifficultyLevel)));
        var currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

        var prompt = $@"
        Analyze this user task: ""{taskDescription}""
        
        1. Determine the Difficulty (Easy/Medium/Hard).
        2. Determine the exact Category from this list: [{categories}].
           Use these guidelines to choose the best fit:
           
           [WARRIOR TYPES]
           - 'Sport': Gym, running, physical exercise.
           - 'Career': Job tasks, meetings, earning money, professional work.
           - 'Discipline': Waking up early, sticking to schedule, breaking bad habits.

           [MAGE TYPES]
           - 'Education': Studying, university, courses, learning new topics.
           - 'Reading': Reading books, articles, documentation.
           - 'Tech': Programming, fixing computer, configuring software, IT tasks.

           [CRAFTER TYPES]
           - 'Art': Drawing, music, writing, design, creativity.
           - 'Chores': Cleaning, cooking, buying groceries, laundry, household repairs.
           - 'Hobbies': Gaming, watching movies, collecting, leisure activities.

           [HEALER TYPES]
           - 'Health': Doctor appointments, taking medicine, dentist.
           - 'Family': Spending time with relatives, calling parents, helping kids.
           - 'SelfCare': Meditation, sleep, mental health, spa, relaxing.

        Return ONLY raw JSON (no Markdown):
        {{
            ""difficulty"": ""Medium"",
            ""category"": ""Tech""
        }}
    ";

        try
        {
            var response = await _aiService.GetAnswerAsync(prompt);
            _logger.LogInformation($"[AI RAW RESPONSE]: {response}");

            // Очистка відповіді від можливих маркерів Markdown, якщо AI їх все ж додав
            var cleanJson = response
                .Replace("```json", "")
                .Replace("```", "")
                .Trim();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() } // Автоматично конвертує рядки в Enum
            };

            var result = JsonSerializer.Deserialize<AiAnalysisResult>(cleanJson, options);

            return result ?? new AiAnalysisResult 
            { 
                Difficulty = DifficultyLevel.Medium, 
                Category = TaskCategory.None, // Дефолтна категорія
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[AI ANALYZER ERROR]");
            // Fallback (резервні дані, якщо AI впав)
            return new AiAnalysisResult 
            { 
                Difficulty = DifficultyLevel.Medium, 
                Category = TaskCategory.None, // Змініть на вашу дефолтну категорію
            };
        }
    }
}