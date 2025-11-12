using MediatR;

namespace QuestLog.Application.Feature.Users.Commands;

public class DeleteTaskCommand: IRequest
{
    public Guid TaskId { get; set; }
    public Guid AvatarId { get; set; }
    
}