using MediatR;
using QuestLog.Application.Dto;

namespace QuestLog.Application.Feature.Users.Queries;

public class GetTaskByAvatarQuery: IRequest<IEnumerable<TaskDto>>
{
    public Guid AvatarId { get; set; }
}