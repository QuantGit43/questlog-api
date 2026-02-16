using System.Text.Json.Serialization;
using MediatR;
using QuestLog.Domain.Enums;

namespace QuestLog.Application.Feature.Tasks.Commands;

public class CreateTaskCommand : IRequest<Guid>
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public TaskType Type { get; set; }
    public TaskCategory? Category { get; set; } // Користувач може вибрати категорію вручну
    public DifficultyLevel? Difficulty { get; set; } // Збережена складність з AI-прев'ю
    public int? XpReward { get; set; } // Збережений XP
    public int? GoldReward { get; set; } // Збережене Gold
    public DateTime? DueDate { get; set; } // Якщо користувач вибрав дату вручну
}