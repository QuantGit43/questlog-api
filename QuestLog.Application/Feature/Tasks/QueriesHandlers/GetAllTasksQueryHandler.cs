using MediatR;
using QuestLog.Application.Common.Models; 
using QuestLog.Application.Dto;
using QuestLog.Application.Feature.Tasks.Queries;
using QuestLog.Domain.Enums; 
using QuestLog.Domain.Interfaces;
using QuestLog.Application.Common.Extensions;

namespace QuestLog.Application.Feature.Tasks.QueriesHandlers;

public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, PagedList<TaskDto>>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUserContext _userContext;

    public GetAllTasksQueryHandler(ITaskRepository taskRepository, IUserContext userContext)
    {
        _taskRepository = taskRepository;
        _userContext = userContext;
    }

    public async Task<PagedList<TaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
    {
        var query = _taskRepository.GetQueryable();
        var currentUserId = _userContext.UserId;

        query = query.Where(t => t.Avatar.UserId == currentUserId);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(t => t.Title.Contains(request.SearchTerm));
        }

        if (request.IsCompleted.HasValue)
        {
            query = query.Where(t => t.IsCompleted == request.IsCompleted.Value);
        }

        query = request.SortBy switch
        {
            SortBy.CreatedAtAsc => query.OrderBy(t => t.CreatedAt),
            SortBy.CreatedAtDesc => query.OrderByDescending(t => t.CreatedAt),
            
            SortBy.TitleAsc => query.OrderBy(t => t.Title),
            SortBy.TitleDesc => query.OrderByDescending(t => t.Title),
            
            SortBy.XpRewardDesc => query.OrderByDescending(t => t.XPReward),
            SortBy.GoldRewardDesc => query.OrderByDescending(t => t.GoldReward),
            
            _ => query.OrderByDescending(t => t.CreatedAt) 
        };

        var dtoQuery = query.Select(t => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Type = t.Type,
            IsCompleted = t.IsCompleted,
            XPReward = t.XPReward,
            GoldReward = t.GoldReward,
            DueDate = t.DueDate,
            CreatedAt = t.CreatedAt, 
            AvatarId = t.AvatarId
        });

        return await dtoQuery.ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}