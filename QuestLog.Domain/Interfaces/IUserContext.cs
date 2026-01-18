namespace QuestLog.Domain.Interfaces;

public interface IUserContext
{
    Guid UserId { get; }
    Guid AvatarId { get; }
    bool IsAuthenticated { get; }
    bool IsAdmin { get; }
}
    