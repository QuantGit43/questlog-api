using QuestLog.Application.Dto;

namespace QuestLog.Application.Interfaces;

public interface ITaskAnalyser
{
    Task<AiAnalysisResult> AnalyseAsync(string taskDescription);
}