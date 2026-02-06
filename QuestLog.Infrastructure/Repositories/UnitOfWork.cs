using QuestLog.Domain.Interfaces;
using QuestLog.Infrastructure.Data;

namespace QuestLog.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly QuestLogDbContext _context;
    public IAvatarRepository Avatars { get; }
    public IUserRepository Users { get; }
    public ITaskRepository Tasks { get; }
    public IItemRepository Items { get; }
    public IInventoryRepository Inventory { get; }
    
    public UnitOfWork(
        QuestLogDbContext context, 
        IUserRepository userRepository,
        IAvatarRepository avatarRepository,
        ITaskRepository taskRepository,
        IItemRepository itemRepository,
        IInventoryRepository inventoryRepository)
    {
        _context = context;
        
        Avatars = avatarRepository;
        Users = userRepository;
        Tasks = taskRepository;
        Items = itemRepository;
        Inventory = inventoryRepository;
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
        
    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}