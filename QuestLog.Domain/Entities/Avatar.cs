using QuestLog.Domain.Enums;

namespace QuestLog.Domain.Entities;

public class Avatar
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public User? User { get; private set; }
    
    public string? Name { get; private set; }
    public AvatarClass Class { get; private set; }
    public int Level { get; private set; }
    
    public long XP { get; private set; }
    public int HP { get; private set; }
    public int MaxHP { get; private set; }
    public int Gold { get; private set; }
    public virtual ICollection<Task>  Tasks { get; private set; } = new List<Task>();

    private Avatar() 
    {
        Tasks = new HashSet<Task>();
    }
    public Avatar(string name, AvatarClass avatarClass)
    {
        Id = Guid.NewGuid();
        Name = name;
        Class = avatarClass;
        Level = 1;
        XP = 0;
        MaxHP = 100; 
        HP = MaxHP;
        Gold = 0;
        
        Tasks = new HashSet<Task>();

    }
    
    public void AddExperience(long amount)
    {
        if (amount < 0) return;
        XP += amount;
    }

    public void AddGold(int amount)
    {
        if (amount < 0) return;
        Gold += amount;
    }
    public void TakeDamage(int amount)
    {
        if (amount < 0) return; 

        HP -= amount;

        if (HP < 0)
        {
            HP = 0;
        }
    }
    public void Heal(int amount)
    {
        if (amount < 0) return; 

        HP += amount;

        if (HP > MaxHP)
        {
            HP = MaxHP;
        }
    }
    public void ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName) || newName.Length > 50)
        {
            throw new ArgumentException("The name cannot be empty or longer than 50 characters.\n");
        }
        Name = newName;
    }
    
}