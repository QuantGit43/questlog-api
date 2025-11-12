using MediatR;
using QuestLog.Application.Dto;
using QuestLog.Application.Feature.Users.Queries;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Users.QueriesHandlers;

public class GetTaskByAvatarQueryHandler: IRequestHandler<GetTaskByAvatarQuery, IEnumerable<TaskDto>>
{
    private readonly ITaskRepository _taskRepository;
    
    public GetTaskByAvatarQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<IEnumerable<TaskDto>> Handle(GetTaskByAvatarQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _taskRepository.GetTasksByAvatarIdAsync(request.AvatarId);

        return tasks.Select(q => new TaskDto
        {
            Id = q.Id,
            Title = q.Title,
            Description = q.Description,
            Type = q.Type,
            IsCompleted = q.IsCompleted,
            XPReward = q.XPReward,
            GoldReward = q.GoldReward,
            DueDate = q.DueDate,
        });
    }
}