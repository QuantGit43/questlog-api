using System.ComponentModel.DataAnnotations;
using QuestLog.Domain.Enums;

namespace QuestLog.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    
    [Required]   
    [MaxLength(50)]
    public string? Username { get; private set; }
    
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; private set; }
    
    [Required]
    public string? PasswordHash { get; private set; } 
    
    public Guid AvatarId { get; private set; }
    public virtual Avatar? Avatar { get; private set; }
    
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    
    public UserRole Role { get; private set; } = UserRole.User;
    
    protected User() {}

    public User(string? username, string? email, string? passwordHash, Avatar? avatar = null)
    {
        Id = Guid.NewGuid();
        Username = username;
        Email = email;
        PasswordHash = passwordHash; 
        Avatar = avatar;
        if (avatar != null) 
            AvatarId = avatar.Id;
    }
    
    public void UpdateProfile(string username, string email)
    {
        if (string.IsNullOrWhiteSpace(username) || username.Length > 50)
        {
            throw new ArgumentException("Username cannot be empty or longer than 50 characters.\n");
        }
        if (string.IsNullOrWhiteSpace(email) || email.Length > 100) 
        {
            throw new ArgumentException("Email cannot be empty or longer than 100 characters.\n");
        }

        Username = username;
        Email = email;
    }
    public void PromoteToAdmin()
    {
        Role = UserRole.Admin;
    }
}