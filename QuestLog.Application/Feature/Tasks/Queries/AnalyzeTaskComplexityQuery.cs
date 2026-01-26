using MediatR;
using QuestLog.Application.Dto;

namespace QuestLog.Application.Feature.Tasks.Queries;

public record AnalyzeTaskComplexityQuery(string Title, string? Description) : IRequest<TaskComplexityDto>;