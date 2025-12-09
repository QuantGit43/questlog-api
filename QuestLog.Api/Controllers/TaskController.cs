using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestLog.Application.Feature.Tasks.Commands;
using QuestLog.Application.Feature.Tasks.Queries;

namespace QuestLog.Api.Controllers;

[ApiController ][Authorize]
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
    public async Task<IActionResult> DeleteTask([FromBody] DeleteTaskCommand command)
    {
           await _sender.Send(command);
           return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTasks()
    {
            var query = new GetAllTasksQuery();
            var tasks = await _sender.Send(query);
          return Ok(tasks);
    }
}