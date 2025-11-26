using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuestLog.Application.Feature.Auth.Commands;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        await _sender.Send(command);
        
        return Ok(); 
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserCommand command)
    {
        var authResult = await _sender.Send(command);
        
        return Ok(authResult);
    }
}