using Microsoft.EntityFrameworkCore;
using QuestLog.Domain.Interfaces;
using QuestLog.Infrastructure.Data;
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

    public async Task<IEnumerable<Task>> GetIncompleteTasksAsync(Guid avatarId)
    {
        return await _context.Tasks
            .Where(q => q.OwnerAvatarId == avatarId && !q.IsCompleted)
            .AsNoTracking()
            .ToListAsync();
    }
}