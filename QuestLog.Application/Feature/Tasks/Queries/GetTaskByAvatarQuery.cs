using MediatR;
using QuestLog.Application.Common.Models;
using QuestLog.Application.Dto;

namespace QuestLog.Application.Feature.Tasks.Queries;

public class GetTaskByAvatarQuery: IRequest<PagedList<TaskDto>>
{
    public Guid AvatarId { get; set; }
    
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}