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
        var taskId = await _sender.Send(command);
        var query = new GetTaskByIdQuery { TaskId = taskId };
        var createdTask = await _sender.Send(query);

        return CreatedAtAction(nameof(GetTaskById), new { taskId = taskId }, createdTask);
    }
    
    [HttpPost("analyze-complexity")]
    public async Task<IActionResult> AnalyzeComplexity([FromBody] AnalyzeComplexityRequest request)
    {
        var query = new AnalyzeTaskComplexityQuery(request.Title, request.Description);
        var result = await _sender.Send(query);
        return Ok(result);
    }

    [HttpGet("/api/avatars/{avatarId:guid}/tasks")]
    public async Task<IActionResult> GetTasksForAvatar(Guid avatarId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetTaskByAvatarQuery 
        { 
            AvatarId = avatarId,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        
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
    
    [HttpPost("{id}/complete")]
    public async Task<IActionResult> Complete(Guid id)
    {
        var result = await _sender.Send(new CompleteTaskCommand { TaskId = id });
    
        return Ok(result);
    }
}

public record AnalyzeComplexityRequest(string Title, string? Description);