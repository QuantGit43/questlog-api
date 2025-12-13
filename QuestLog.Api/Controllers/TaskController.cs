using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuestLog.Application.Feature.Tasks.Commands;
using QuestLog.Application.Feature.Tasks.Queries;
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
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand command)
    {
            var taskid = await _sender.Send(command);
            return Ok(new {TaskId = taskid});
    }

    [HttpGet("/api/avatars/{avatarId:guid}/tasks")]
    public async Task<IActionResult> GetTasksForAvatar(Guid avatarId)
    {
            var query = new GetTaskByAvatarQuery{AvatarId = avatarId};
            var tasks = await _sender.Send(query);
            return Ok(tasks);
    }

    [HttpGet("{taskId:guid}")]
    public async Task<IActionResult> GetTaskById(Guid taskId)
    {
           var query = new GetTaskByIdQuery{TaskId = taskId};
           return Ok(await _sender.Send(query));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskCommand command)
    {
            await _sender.Send(command);
            return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
           var command = new DeleteTaskCommand { TaskId = id };
           await _sender.Send(command);
           return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTasks([FromQuery] GetAllTasksQuery query)
    {  
          var tasks = await _sender.Send(query);
          return Ok(tasks);
    }
}