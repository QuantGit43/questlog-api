using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuestLog.Application.Dto;
using QuestLog.Application.Feature.Users.Commands;
using QuestLog.Application.Feature.Users.Queries;

namespace QuestLog.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly ISender _sender;

        public UsersController(ISender sender)
        {
            _sender = sender;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserCommand command)
        {
            try
            {
                var userId = await _sender.Send(command);
                return Ok(new { UserId = userId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var query = new GetUserByIdQuery { UserId = id };
                UserDto userDto = await _sender.Send(query);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                return Ok(await _sender.Send(new GetAllUsersQuery()));
            }
            catch (Exception ex)
            {
               return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpDelete(("{id:guid}"))]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                await _sender.Send(new DeleteUserCommand{UserId = id});
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
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
}