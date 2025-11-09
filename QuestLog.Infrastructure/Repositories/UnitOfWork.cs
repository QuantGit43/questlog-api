using QuestLog.Domain.Interfaces;
using QuestLog.Infrastructure.Data;

namespace QuestLog.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly QuestLogDbContext _context;
    
    public IAvatarRepository Avatars { get; }
    public IUserRepository Users { get; }
    public ITaskRepository Tasks { get; }
    
    public UnitOfWork(QuestLogDbContext context)
    {
        _context = context;
        
        Avatars = new AvatarRepository(_context);
        Users = new UserRepository(_context);
        Tasks = new TaskRepository(_context);
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }
        
    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}