using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestLog.Application.Dto;
using QuestLog.Application.Feature.Users.Commands;
using QuestLog.Application.Feature.Users.Queries;

namespace QuestLog.Api.Controllers
{
    [ApiController][Authorize]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly ISender _sender;

        public UsersController(ISender sender)
        {
            _sender = sender;
        }
        
        /*[HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserCommand command)
        {
                var userId = await _sender.Send(command);
                return Ok(new { UserId = userId });
        }*/
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
                var query = new GetUserByIdQuery { UserId = id };
                UserDto userDto = await _sender.Send(query);
                return Ok(userDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
                return Ok(await _sender.Send(new GetAllUsersQuery()));
        }

        [HttpDelete(("{id:guid}"))]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
                await _sender.Send(new DeleteUserCommand{UserId = id});
                return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        {
              await _sender.Send(command);
              return Ok();
        }
    }
}