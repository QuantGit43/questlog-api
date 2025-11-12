using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuestLog.Application.Feature.Users.Commands;
using QuestLog.Application.Feature.Users.Queries;

namespace QuestLog.Api.Controllers;

[ApiController ]
[Route("api/tasks")]
public class TaskController: ControllerBase
{
    private readonly ISender _sender;
    
    public TaskController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateQuest([FromBody] CreateTaskCommand command)
    {
        try
        {
            var taskid = await _sender.Send(command);
            return Ok(new {TaskId = taskid});
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet("/api/avatars/{avatarId:guid}/tasks")]
    public async Task<IActionResult> GetTasksForAvatar(Guid avatarId)
    {
        try
        {
            var query = new GetTaskByAvatarQuery{AvatarId = avatarId};
            var tasks = await _sender.Send(query);
            return Ok(tasks);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet("{taskId:guid}")]
    public async Task<IActionResult> GetTaskById(Guid taskId)
    {
        try
        {
           var query = new GetTaskByIdQuery{TaskId = taskId};
           return Ok(await _sender.Send(query));
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskCommand command)
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

    [HttpDelete]
    public async Task<IActionResult> DeleteTask([FromBody] DeleteTaskCommand command)
    {
        try
        {
           await _sender.Send(command);
           return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTasks()
    {
        try
        {
            var query = new GetAllTasksQuery();
            var tasks = await _sender.Send(query);
          return Ok(tasks);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}