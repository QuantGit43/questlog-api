using QuestLog.Domain.Entities; 

namespace QuestLog.Domain.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}