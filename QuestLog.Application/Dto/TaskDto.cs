using QuestLog.Domain.Enums;

namespace QuestLog.Application.Dto;

public class TaskDto
{
    public Guid Id { get; set; }
    
    public Guid AvatarId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public TaskType Type { get; set; }
    public DifficultyLevel Difficulty { get; set; }
    public bool IsCompleted { get; set; }
    public int XPReward {get; set;}
    public int GoldReward {get; set;}
    public DateTime? DueDate {get; set;}
    
    public DateTime CreatedAt { get; set; }
}