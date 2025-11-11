using MediatR;
using QuestLog.Domain.Enums;

namespace QuestLog.Application.Feature.Users.Commands;

public record CreateUserCommand: IRequest<Guid>
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    
    public string? AvatarName { get; set; }
    public AvatarClass AvatarClass { get; set; }
}