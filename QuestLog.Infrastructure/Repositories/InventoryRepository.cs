using Microsoft.EntityFrameworkCore;
using QuestLog.Domain.Entities;
using QuestLog.Domain.Interfaces;
using QuestLog.Infrastructure.Data;
using Task = System.Threading.Tasks.Task;

namespace QuestLog.Infrastructure.Repositories;

public class InventoryRepository: Repository<Inventory>, IInventoryRepository
{
    public InventoryRepository(QuestLogDbContext context): base(context)
    {
    }

    public async Task<List<Inventory>> GetAvatarInventoryAsync(Guid avatarId)
    {
        return await _context.Inventory
            .Include(x => x.Item)
            .Where(x => x.AvatarId == avatarId)
            .ToListAsync();
    }
    public async Task<Inventory?> GetByAvatarAndItemAsync(Guid avatarId, Guid itemId)
    {
        return await _context.Inventory
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.AvatarId == avatarId && x.ItemId == itemId);
    }
    
    public async Task AddAsync(Inventory inventory)
    {
        await _context.Inventory.AddAsync(inventory);
    }
    
    public void Remove(Inventory inventory)
    {
        _context.Inventory.Remove(inventory);
    }
    
    public void Update(Inventory inventory)
    {
        _context.Inventory.Update(inventory);
    }
}