using QuestLog.Domain.Entities;

namespace QuestLog.Domain.Interfaces;

public interface IAvatarRepository: IRepository<Avatar>
{
    Task<Avatar> GetByUserIdAsync(Guid userId);
}