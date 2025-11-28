using System.ComponentModel.DataAnnotations;
using QuestLog.Domain.Enums;

namespace QuestLog.Domain.Entities;

public class Task
{
    public Guid Id { get; set; }
    
    [Required]
    public Guid OwnerAvatarId { get; set; }
    public virtual Avatar Avatar { get; private set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; }
    
    [MaxLength(500)]
    public string Description { get; set; }
    
    [Required]
    public TaskType Type { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CreatedAt { get; set; }
    
    [Range(0, 1000000)]
    public int XPReward { get; set; }
    
    [Range(0, 1000000)]
    public int GoldReward { get; set; }
    
    public DateTime? DueDate { get; set; }
    
    
    private Task() { }
    
    public Task(Guid ownerAvatarId, string title, TaskType type, int xpReward, int goldReward, string description = "", DateTime? dueDate = null)
    {
        if (xpReward < 0 || goldReward < 0)
        {
            throw new ArgumentException("Awards cannot be negative.\n");
        }
            
        Id = Guid.NewGuid();
        OwnerAvatarId = ownerAvatarId;
        Title = title;
        Type = type;
        XPReward = xpReward;
        GoldReward = goldReward;
        Description = description;
        DueDate = dueDate;
        IsCompleted = false;
    }
    public void Complete()
    {
        if (!IsCompleted)
        {
            IsCompleted = true;
        }
    }
    public void UpdateDetails(string title, string description, int xpReward, int goldReward)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length > 100)
        {
            throw new ArgumentException("The title cannot be empty or longer than 100 characters.\n");
        }
        if (xpReward < 0 || goldReward < 0)
        {
            throw new ArgumentException("Awards cannot be negative.\n");
        }

        Title = title;
        Description = description;
        XPReward = xpReward;
        GoldReward = goldReward;
    }
}