using MediatR;

namespace QuestLog.Application.Feature.Auth.Commands;

public class LoginUserCommand : IRequest<AuthResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public record AuthResponse
{
    public string UserId { get; set; }
    public string Username { get; set; }
    public string Token { get; set; }
    
    public string Role { get; set; }
}