using Microsoft.EntityFrameworkCore;
using QuestLog.Domain.Entities;
using QuestLog.Domain.Interfaces;
using QuestLog.Infrastructure.Data;

namespace QuestLog.Infrastructure.Repositories;

public class UserRepository: Repository<User>, IUserRepository
{
    public UserRepository(QuestLogDbContext context) : base(context){}

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public new async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .Include(u => u.Avatar)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
}