using QuestLog.Domain.Entities;
using Task = QuestLog.Domain.Entities.Task;

namespace QuestLog.Domain.Interfaces;

public interface IInventoryRepository : IRepository<Inventory>
{
    Task<List<Inventory>> GetAvatarInventoryAsync(Guid avatarId);
    System.Threading.Tasks.Task AddAsync(Inventory inventoryItem);
    void Remove(Inventory inventoryItem);
    void Update(Inventory inventoryItem);
    Task<Inventory?> GetByAvatarAndItemAsync(Guid avatarId, Guid itemId);
}