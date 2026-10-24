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
    private readonly IAvatarRepository _avatarRepository;
    private readonly IUserContext _userContext;

    public GetTasksByAvatarQueryHandler(ITaskRepository taskRepository, IAvatarRepository avatarRepository, IUserContext userContext)
    {
        _taskRepository = taskRepository;
        _avatarRepository = avatarRepository;
        _userContext = userContext;
    }

    public async Task<PagedList<TaskDto>> Handle(GetTaskByAvatarQuery request, CancellationToken cancellationToken)
    {
        var avatar = await _avatarRepository.GetByIdAsync(request.AvatarId);
        if (avatar == null)
        {
            throw new KeyNotFoundException($"Аватар з ID {request.AvatarId} не знайдений.");
        }
        if (avatar.UserId != _userContext.UserId && !_userContext.IsAdmin)
        {
            throw new UnauthorizedAccessException("Ви не можете переглядати завдання чужого аватара.");
        }

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