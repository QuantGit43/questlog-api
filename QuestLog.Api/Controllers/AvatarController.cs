using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuestLog.Application.Feature.Users.Commands;
using QuestLog.Application.Feature.Users.Queries;

namespace QuestLog.Api.Controllers;

[ApiController ]
[Route("api/avatars")]
public class AvatarController: ControllerBase
{
    private readonly ISender  _sender;

    public AvatarController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAvatarById(Guid id)
    {
        try
        {
            var query = new GetAvatarByIdQuery{AvatarId = id};
            var avatarDto = await _sender.Send(query);
            return Ok(avatarDto);
        }
        catch (Exception ex )
        {
            return NotFound(new { Error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAvatars()
    {
        try
        {
          var avatars = await _sender.Send(new GetAllAvatarsQuery());
          return Ok(avatars);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAvatar([FromBody] UpdateAvatarCommand command)
    {
        try
        {
            await _sender.Send(command);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}