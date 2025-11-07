using QuestLog.Domain.Entities;
using Task = QuestLog.Domain.Entities.Task;

namespace QuestLog.Domain.Interfaces;

public interface ITaskRepository: IRepository<Task>
{
    Task<IEnumerable<Task>> GetTasksByAvatarIdAsync(Guid avatarId);
    Task<IEnumerable<Task>> GetIncompleteTasksAsync(Guid avatarId);
}