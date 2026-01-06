using MediatR;

namespace QuestLog.Application.Feature.Auth.Commands;

public class RegisterUserCommand : IRequest<AuthResponse>
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}