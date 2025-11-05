using QuestLog.Domain.Enums;

namespace QuestLog.Domain.Entities;

public class Task
{
    public Guid Id { get; set; }
    public Guid OwnerAvatarId { get; set; }
    public virtual Avatar Owner { get; set; }
    
    public string Title { get; set; }
    public string Description { get; set; }
    public TaskType Type { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CreatedAt { get; set; }
    
    public int XPReward { get; set; }
    public int GoldReward { get; set; }
    
    public DateTime? DueDate { get; set; }
}