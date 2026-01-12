using System.Security.Claims; // Не забудьте додати цей using
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestLog.Api.Exceptions;
using QuestLog.Api.Extentions;
using QuestLog.Application.Feature.Tasks.Commands;
using QuestLog.Application.Feature.Tasks.Queries;

namespace QuestLog.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/tasks")]
public class TaskController : ControllerBase
{
    private readonly ISender _sender;

    public TaskController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand command)
    {
        // 1. Витягуємо ID поточного користувача з JWT токена
        // Це гарантує, що завдання створюється саме для того, хто залогінився
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdString))
        {
            return Unauthorized();
        }

        // 2. Передаємо цей ID в команду
        // (Переконайтеся, що ви додали властивість UserId до класу CreateTaskCommand)
        command.UserId = Guid.Parse(userIdString);

        // 3. Відправляємо команду. AvatarId тепер знайдеться всередині хендлера автоматично.
        var taskId = await _sender.Send(command);
        
        return Ok(new { TaskId = taskId });
    }

    [HttpGet("/api/avatars/{avatarId:guid}/tasks")]
    public async Task<IActionResult> GetTasksForAvatar(Guid avatarId)
    {
        var query = new GetTaskByAvatarQuery { AvatarId = avatarId };
        var tasks = await _sender.Send(query);
        return Ok(tasks);
    }

    [HttpGet("{taskId:guid}")]
    public async Task<IActionResult> GetTaskById(Guid taskId)
    {
        var query = new GetTaskByIdQuery { TaskId = taskId };
        return Ok(await _sender.Send(query));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskCommand command)
    {
        await _sender.Send(command);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
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