using MediatR;
using QuestLog.Application.Dto;

namespace QuestLog.Application.Feature.Tasks.Queries;

public class GetTaskByIdQuery: IRequest<TaskDto>
{
    public Guid TaskId { get; set; }
}