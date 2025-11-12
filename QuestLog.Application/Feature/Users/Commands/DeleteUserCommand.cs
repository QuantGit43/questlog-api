using MediatR;

namespace QuestLog.Application.Feature.Users.Commands;

public class DeleteUserCommand: IRequest
{
    public Guid UserId { get; set; }
}