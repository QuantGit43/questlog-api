using Microsoft.EntityFrameworkCore;
using QuestLog.Domain.Enums;
using QuestLog.Domain.Interfaces;
using QuestLog.Infrastructure.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Task = QuestLog.Domain.Entities.Task;

namespace QuestLog.Infrastructure.Repositories;

public class TaskRepository: Repository<Task>, ITaskRepository
{
    public TaskRepository(QuestLogDbContext context) : base(context){}

    public async Task<IEnumerable<Task>> GetTasksByAvatarIdAsync(Guid avatarId)
    {
        return await _context.Tasks
            .Where(q => q.OwnerAvatarId == avatarId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Task>> GetTasksWithFiltersAsync(string? searchTerm, bool? isCompleted,
        SortBy? sortOption)
    {
        var query = _context.Tasks.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(t => EF.Functions.ILike(t.Title, $"%{searchTerm}%"));
        }

        if (isCompleted.HasValue)
        {
            query = query.Where(t => t.IsCompleted == isCompleted.Value);
        }

        query = sortOption switch
        {
            SortBy.CreatedAtAsc => query.OrderBy(t => t.CreatedAt),
            SortBy.CreatedAtDesc => query.OrderByDescending(t => t.CreatedAt),
            SortBy.TitleAsc => query.OrderBy(t => t.Title),
            SortBy.TitleDesc => query.OrderByDescending(t => t.Title),
            SortBy.XpRewardDesc => query.OrderByDescending(t => t.XPReward),
            SortBy.GoldRewardDesc => query.OrderByDescending(t => t.GoldReward),
            _ => query.OrderByDescending(t => t.CreatedAt)
        };

        return await query.ToListAsync();
    }
}