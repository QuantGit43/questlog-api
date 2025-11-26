using QuestLog.Domain.Interfaces;
using BCrypt.Net;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        // Створюємо хеш
        public string Hash(string password)
        {
            // BCrypt автоматично генерує "сіль" (salt)
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Перевіряємо хеш
        public bool Verify(string passwordHash, string providedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(providedPassword, passwordHash);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}