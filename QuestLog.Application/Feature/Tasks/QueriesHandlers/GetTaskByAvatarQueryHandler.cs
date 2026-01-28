using MediatR;
using QuestLog.Application.Common.Extensions; 
using QuestLog.Application.Common.Models;
using QuestLog.Application.Dto;
using QuestLog.Application.Feature.Tasks.Queries;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Tasks.QueriesHandlers;

public class GetTasksByAvatarQueryHandler : IRequestHandler<GetTaskByAvatarQuery, PagedList<TaskDto>>
{
    private readonly ITaskRepository _taskRepository;

    public GetTasksByAvatarQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<PagedList<TaskDto>> Handle(GetTaskByAvatarQuery request, CancellationToken cancellationToken)
    {
        var query = _taskRepository.GetQueryable();

        query = query.Where(t => t.AvatarId == request.AvatarId);

        query = query.OrderByDescending(t => t.CreatedAt);

        var dtoQuery = query.Select(t => new TaskDto 
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Type = t.Type,
            Difficulty = t.Difficulty,
            Category = t.Category,
            IsCompleted = t.IsCompleted,
            CreatedAt = t.CreatedAt,
            XPReward = t.XPReward,
            GoldReward = t.GoldReward,
            DueDate = t.DueDate,
            AvatarId = t.AvatarId
        });

        return await dtoQuery.ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}