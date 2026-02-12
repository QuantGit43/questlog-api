using QuestLog.Domain.Entities;
using Task = QuestLog.Domain.Entities.Task;

namespace QuestLog.Domain.Interfaces;

public interface IItemRepository : IRepository<Item>
{
    Task<Item?> GetByIdAsync(Guid itemId);
    Task<List<Item>> GetAllAsync();
    System.Threading.Tasks.Task AddAsync(Item item);
}