using MediatR;
using QuestLog.Application.Dto;
using QuestLog.Application.Feature.Tasks.Queries;
using QuestLog.Application.Feature.Users.Queries;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Tasks.QueriesHandlers;

public class GetTaskByIdQueryHandler: IRequestHandler<GetTaskByIdQuery, TaskDto>
{
    public readonly ITaskRepository _taskRepository;

    public GetTaskByIdQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var query = await _taskRepository.GetByIdAsync(request.TaskId);
        if (query == null)
        {
            throw new KeyNotFoundException($"Завдання з ID {request.TaskId} не знайдено.");
        }

        return new TaskDto
        {
            Id = query.Id,
            Description = query.Description,
            Title = query.Title,
            Type = query.Type,
            IsCompleted = query.IsCompleted,
            XPReward = query.XPReward,
            GoldReward = query.GoldReward,
            DueDate = query.DueDate
        };
    }
}