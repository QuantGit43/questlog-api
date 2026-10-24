using MediatR;
using QuestLog.Application.Dto;
using QuestLog.Application.Feature.Tasks.Queries;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Tasks.QueriesHandlers;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IAvatarRepository _avatarRepository;
    private readonly IUserContext _userContext;

    public GetTaskByIdQueryHandler(ITaskRepository taskRepository, IAvatarRepository avatarRepository, IUserContext userContext)
    {
        _taskRepository = taskRepository;
        _avatarRepository = avatarRepository;
        _userContext = userContext;
    }

    public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId);

        if (task == null)
        {
            throw new KeyNotFoundException($"Завдання з ID {request.TaskId} не знайдено.");
        }

        var avatar = await _avatarRepository.GetByIdAsync(task.AvatarId);
        if (avatar?.UserId != _userContext.UserId && !_userContext.IsAdmin)
        {
            throw new UnauthorizedAccessException("Це завдання вам не належить.");
        }

        return new TaskDto
        {
            Id = task.Id,
            AvatarId = task.AvatarId, 
            Title = task.Title,
            Description = task.Description,
            Type = task.Type,
            Difficulty = task.Difficulty,
            Category = task.Category, 
            IsCompleted = task.IsCompleted,
            CreatedAt = task.CreatedAt,    
            XPReward = task.XPReward,
            GoldReward = task.GoldReward,
            DueDate = task.DueDate
        };
    }
}