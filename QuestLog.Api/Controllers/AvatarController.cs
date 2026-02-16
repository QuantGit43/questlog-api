using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestLog.Application.Feature.Avatars.Commands;
using QuestLog.Application.Feature.Avatars.Queries;
using QuestLog.Application.Feature.Users.Commands;
using QuestLog.Application.Feature.Users.Queries;

namespace QuestLog.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/avatars")]
public class AvatarController : ControllerBase
{
    private readonly ISender _sender;

    public AvatarController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAvatarCommand command)
    {
        var avatarId = await _sender.Send(command);
        return Ok(new { AvatarId = avatarId });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAvatarById(Guid id)
    {
        var query = new GetAvatarByIdQuery { AvatarId = id };
        var avatarDto = await _sender.Send(query);
        return Ok(avatarDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllAvatarsQuery query)
    {
        var result = await _sender.Send(query);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAvatar([FromBody] UpdateAvatarCommand command)
    {
        await _sender.Send(command);
        return Ok();
    }

    [HttpPost("visuals/toggle-gender")]
    public async Task<IActionResult> ToggleGender()
    {
        await _sender.Send(new ToggleGenderCommand());
        return Ok(new { message = "Стать змінено" });
    }

    [HttpGet("visuals/appearance")]
    public async Task<IActionResult> GetAppearance()
    {
        var result = await _sender.Send(new GetAvatarAppearanceQuery());
        return Ok(result);
    }

    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentAvatar()
    {
        var query = new GetCurrentAvatarQuery();
        var result = await _sender.Send(query);
        return Ok(result);
    }
}