using Microsoft.EntityFrameworkCore;
using QuestLog.Domain.Entities;
using QuestLog.Domain.Interfaces;
using QuestLog.Infrastructure.Data;
using Task = System.Threading.Tasks.Task;

namespace QuestLog.Infrastructure.Repositories;

public class ItemRepository: Repository<Item>,IItemRepository
{
    public ItemRepository(QuestLogDbContext context) : base(context)
    { }

    public async Task<Item?> GetByIdAsync(Guid id)
    {
        return await _context.Items.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Item>> GetAllAsync()
    {
        return await _context.Items.ToListAsync();
    }
    
    public async Task AddAsync(Item item)
    {
        await _context.Items.AddAsync(item);
    }
}