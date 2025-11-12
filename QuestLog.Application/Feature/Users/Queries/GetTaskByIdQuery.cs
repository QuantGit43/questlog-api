using MediatR;
using QuestLog.Application.Dto;

namespace QuestLog.Application.Feature.Users.Queries;

public class GetTaskByIdQuery: IRequest<TaskDto>
{
    public Guid TaskId { get; set; }
}