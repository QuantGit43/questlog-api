namespace QuestLog.Domain.Interfaces;

public interface IUserContext
{
    Guid UserId { get; }
    bool IsAuthenticated { get; }
    bool IsAdmin { get; }
}
    