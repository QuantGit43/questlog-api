using MediatR;
using QuestLog.Application.Dto;
using QuestLog.Application.Feature.Tasks.Queries;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Tasks.QueriesHandlers;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto>
{
    private readonly ITaskRepository _taskRepository;

    public GetTaskByIdQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId); 
        
        if (task == null)
        {
            throw new KeyNotFoundException($"Завдання з ID {request.TaskId} не знайдено.");
        }

        return new TaskDto
        {
            Id = task.Id,
            AvatarId = task.OwnerAvatarId, 
            Title = task.Title,
            Description = task.Description,
            Type = task.Type,
            Difficulty = task.Difficulty,  
            IsCompleted = task.IsCompleted,
            CreatedAt = task.CreatedAt,    
            XPReward = task.XPReward,
            GoldReward = task.GoldReward,
            DueDate = task.DueDate
        };
    }
}