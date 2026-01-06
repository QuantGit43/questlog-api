using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestLog.Application.Interfaces;

namespace QuestLog.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TaskAnalysisController : ControllerBase
{
    private readonly IAiService _aiService;

    public TaskAnalysisController(IAiService aiService)
    {
        _aiService = aiService;
    }

    [HttpPost("estimate-difficulty")]
    public async Task<IActionResult> EstimateDifficulty([FromBody] string taskDescription)
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
        var difficulty = await _aiService.GetAnswerAsync(prompt);
        var cleanResult = difficulty.Trim().Replace(".", "");

        return Ok(new { Task = taskDescription, Difficulty = cleanResult });
    }
}