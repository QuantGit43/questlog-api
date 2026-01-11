using QuestLog.Domain.Enums;

namespace QuestLog.Application.Interfaces;

public interface ITaskDifficultyEvaluator
{
    Task<DifficultyLevel> EvaluateAsync(string taskDescription);
}