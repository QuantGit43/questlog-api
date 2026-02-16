using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuestLog.Domain.Enums;

namespace QuestLog.Domain.Entities;

public class Task
{
    private const int DefaultXp = 10;
    private const int DefaultGold = 5;

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
    
    // Зверніть увагу: я перейменував параметри з 'baseXpReward' на 'xpReward',
    // щоб показати, що це вже фінальне число.
    public Task(Guid avatarId,
        string title,
        TaskType type,
        DifficultyLevel difficultyLevel,
        TaskCategory category ,
        string? description = "",
        int xpReward = DefaultXp,     
        int goldReward = DefaultGold, 
        DateTime? dueDate = null)
    {
        Id = Guid.NewGuid();
        AvatarId = avatarId;
        Title = title;
        Type = type;
        Description = description;
        Difficulty = difficultyLevel;
        
        // --- ЗМІНА ТУТ: Множники видалено ---
        // Ми просто беремо те, що нам дали.
        // Якщо дали 100 XP - записуємо 100 XP.
        
        XPReward = xpReward;
        GoldReward = goldReward;

        // Логіка дати
        DueDate = dueDate.HasValue 
            ? DateTime.SpecifyKind(dueDate.Value, DateTimeKind.Utc) 
            : null;

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

        // Логіку бонусів від характеристик гравця (Strength/Intelligence) залишаємо!
        // Це вже не "множник складності", це "множник прокачки героя".
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
            throw new ArgumentException("Title error", nameof(title));
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