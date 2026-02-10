using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuestLog.Domain.Enums;

namespace QuestLog.Domain.Entities;

public class Task
{
    private const int DefaultBaseXp = 100;
    private const int DefaultBaseGold = 100;

    public Guid Id { get; private set; }
    
    [Required]
    public Guid AvatarId { get; private set; }
    public virtual Avatar Avatar { get; private set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; private set; }
    
    [MaxLength(500)]
    public string Description { get; private set; }
    
    [Required]
    public TaskType Type { get; private set; }
    public DifficultyLevel Difficulty { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    [Range(0, 1000000)]
    public int XPReward { get; private set; }
    
    [Range(0, 1000000)]
    public int GoldReward { get; private set; }
    
    public DateTime? DueDate { get; private set; }
    
    public TaskCategory Category { get; private set; }
    
    private Task() { }
    
    public Task(Guid avatarId,
        string title,
        TaskType type,
        DifficultyLevel difficultyLevel,
        TaskCategory category ,
        string? description = "",
        int baseGoldReward = DefaultBaseGold,
        int baseXpReward = DefaultBaseXp,
        DateTime? dueDate = null)
        {
            
        Id = Guid.NewGuid();
        AvatarId = avatarId;
        Title = title;
        Type = type;
        Description = description;
        Difficulty = difficultyLevel;
        
        var multiplier = difficultyLevel switch
        {
            DifficultyLevel.Easy => 1,
            DifficultyLevel.Medium => 2,
            DifficultyLevel.Hard => 4,
            _ => 2
        };
        
        XPReward = baseXpReward * multiplier;
        GoldReward = baseGoldReward * multiplier;

        DueDate = dueDate.HasValue ? DateTime.SpecifyKind(dueDate.Value, DateTimeKind.Utc) : null;
        CreatedAt = DateTime.UtcNow;
        IsCompleted = false;
        Category = category;
    }
    public (int xp, int gold) Complete(Avatar? avatar)
    {
        if (IsCompleted)
        {
            throw new InvalidOperationException("Task is already completed.");
        }
        if (avatar.Id != AvatarId)
        {
            throw new InvalidOperationException("Avatar does not own this task.");
        }
        int relevantStat = avatar.GetStatValue(this.Category);

        double multiplier = 1.0 + (relevantStat / 100.0);
        
        double calculatedXp = this.XPReward * multiplier;
        
        int finalXp = (int)Math.Round(calculatedXp, MidpointRounding.AwayFromZero);
        
        int finalGold = this.GoldReward;
        
        avatar.GainResources(finalXp, finalGold);
        
        avatar.TrainStat(this.Category);
        
        IsCompleted = true;
        
        return (finalXp, finalGold);
    }

    public void UpdateDetails(string title, string description, int xpReward, int goldReward)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length > 100)
        {
            throw new ArgumentException("The title cannot be empty or longer than 100 characters.", nameof(title));
        }
        if (xpReward < 0 || goldReward < 0)
        {
            throw new ArgumentException("Awards cannot be negative.");
        }

        Title = title;
        Description = description;
        XPReward = xpReward;
        GoldReward = goldReward;
    }
}