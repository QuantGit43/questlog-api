using MediatR;
using QuestLog.Application.Dto;

namespace QuestLog.Application.Feature.Tasks.Commands;

public class CompleteTaskCommand: IRequest<TaskCompletionDto>
{
    public Guid TaskId { get; set; }
}