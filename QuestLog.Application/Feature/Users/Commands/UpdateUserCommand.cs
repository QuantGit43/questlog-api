using MediatR;

namespace QuestLog.Application.Feature.Users.Commands;

public class UpdateUserCommand : IRequest
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
}