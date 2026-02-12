namespace QuestLog.Domain.Interfaces;

public interface IUnitOfWork: IDisposable
{
    IUserRepository Users { get; }
    ITaskRepository Tasks { get; }
    IAvatarRepository Avatars { get; }
    
    IItemRepository Items { get; }
    IInventoryRepository Inventory { get; }
    
    Task<int> CompleteAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}