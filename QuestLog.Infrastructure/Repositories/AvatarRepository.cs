using System.Data.Entity;
using QuestLog.Domain.Entities;
using QuestLog.Domain.Interfaces;
using QuestLog.Infrastructure.Data;

namespace QuestLog.Infrastructure.Repositories;

public class AvatarRepository: Repository<Avatar>, IAvatarRepository
{
    public  AvatarRepository(QuestLogDbContext context): base(context){}

    public async Task<Avatar> GetByUserIdAsync(Guid userId)
    {
        return await _context.Avatars
            .Include(a => a.Tasks)
            .FirstOrDefaultAsync(a => a.UserId == userId);
    }
}