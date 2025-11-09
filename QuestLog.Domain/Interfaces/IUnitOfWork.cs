namespace QuestLog.Domain.Interfaces;

public interface IUnitOfWork: IDisposable
{
    IUserRepository Users { get; }
    ITaskRepository Tasks { get; }
    IAvatarRepository Avatars { get; }
    
    Task<int> CompleteAsync();
}