using MediatR;
using QuestLog.Application.Dto;
using QuestLog.Domain.Enums;

namespace QuestLog.Application.Feature.Tasks.Queries;

public class GetAllTasksQuery: IRequest<IEnumerable<TaskDto>>
{
    public string? SearchTerm { get; set; }
    public bool? IsCompleted { get; set; }
    public SortBy? SortBy { get; set; } 
}