using QuestLog.Domain.Enums;

namespace QuestLog.Application.Dto;

public class AiAnalysisResult
{
    public DifficultyLevel Difficulty { get; set; }
    public TaskCategory Category { get; set; }
}