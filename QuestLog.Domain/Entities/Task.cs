using System.ComponentModel.DataAnnotations;
using QuestLog.Domain.Enums;

namespace QuestLog.Domain.Entities;

public class Task
{
    public Guid Id { get; set; }
    
    [Required]
    public Guid OwnerAvatarId { get; set; }
    public virtual Avatar Owner { get; set; }
    
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
    
    public virtual Avatar Avatar { get; private set; }
    
    private Task() { }
    
    public Task(Guid ownerAvatarId, string title, TaskType type, int xpReward, int goldReward, string description = "", DateTime? dueDate = null)
    {
        if (xpReward < 0 || goldReward < 0)
        {
            throw new ArgumentException("Нагороди не можуть бути від'ємними.");
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
}