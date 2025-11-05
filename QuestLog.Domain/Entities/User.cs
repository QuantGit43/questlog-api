namespace QuestLog.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public Guid AvatarId { get; set; }
    public virtual Avatar Avatar { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}