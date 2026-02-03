using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestLog.Application.Feature.Avatars.Commands;
using QuestLog.Application.Feature.Avatars.Queries;
using QuestLog.Application.Feature.Users.Commands;
using QuestLog.Application.Feature.Users.Queries;

namespace QuestLog.Api.Controllers;

[ApiController ][Authorize]
[Route("api/avatars")]
public class AvatarController: ControllerBase
{
    private readonly ISender  _sender;

    public AvatarController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAvatarCommand command)
    {
        // Медіатр знайде CreateAvatarCommandHandler і виконає його
        var avatarId = await _sender.Send(command);
        
        // Повертаємо ID створеного аватара
        return Ok(new { AvatarId = avatarId });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAvatarById(Guid id)
    {
            var query = new GetAvatarByIdQuery{AvatarId = id};
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
}