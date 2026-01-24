using MediatR;
using QuestLog.Application.Common.Models;
using QuestLog.Application.Dto;
using QuestLog.Domain.Enums;

namespace QuestLog.Application.Feature.Tasks.Queries;

public class GetAllTasksQuery: IRequest<PagedList<TaskDto>>
{
    public string? SearchTerm { get; set; }
    public bool? IsCompleted { get; set; }
    public SortBy? SortBy { get; set; } 
    
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}