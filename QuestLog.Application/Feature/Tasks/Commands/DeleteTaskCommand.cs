using MediatR;

namespace QuestLog.Application.Feature.Tasks.Commands;

public class DeleteTaskCommand: IRequest
{
    public Guid TaskId { get; set; }
    
}