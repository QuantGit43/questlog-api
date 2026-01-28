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
    
    public int Strength { get; set; }
    public int Intellect { get; set; }
    public int Dexterity { get; set; }
    public int Wisdom { get; set; }
    public virtual ICollection<Task>  Tasks { get; private set; } = new List<Task>();

    private Avatar() 
    {
        Tasks = new HashSet<Task>();
    }
    public Avatar(Guid userId)
    {
        Id = Guid.NewGuid();
        Name = "New Name";
        UserId = userId;
        Class = AvatarClass.Warrior;
        Level = 1;
        XP = 0;
        MaxHP = 100; 
        HP = MaxHP;
        Gold = 0;
        Strength = 1; 
        Intellect = 1;
        Dexterity = 1;
        Wisdom = 1;
        
        Tasks = new HashSet<Task>();
        
    }
    public void ChooseClass(string name, AvatarClass newClass, int maxHp, int str, int intel, int dex, int wis)
    {
        Name = name;
        Class = newClass;

        MaxHP = maxHp;
        HP = maxHp; 
        
        Strength = str;
        Intellect = intel;
        Dexterity = dex;
        Wisdom = wis;
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
    public void UpdateDetails(string name, AvatarClass avatarClass)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Ім'я аватара не може бути порожнім.");
            
        if (name.Length > 50) 
            throw new ArgumentException("Ім'я занадто довге.");
        Class = avatarClass;
        Name = name;
    }

    public int GetStatValue(TaskCategory category)
    {
        return category switch
        {
            TaskCategory.Sport or TaskCategory.Career or TaskCategory.Discipline => Strength,
            TaskCategory.Education or TaskCategory.Reading or TaskCategory.Tech => Intellect,
            TaskCategory.Art or TaskCategory.Chores or TaskCategory.Hobbies => Dexterity,
            TaskCategory.Health or TaskCategory.Family or TaskCategory.SelfCare => Wisdom,
            _ => 0
        };
    }
    public void GainResources(int xp, int gold)
    {
        XP += xp;
        Gold += gold;
    }
    
    public void TrainStat(TaskCategory category)
    {
        switch (category)
        {
            case TaskCategory.Sport:
            case TaskCategory.Career:
            case TaskCategory.Discipline:
                Strength++; 
                break;

            case TaskCategory.Education:
            case TaskCategory.Reading:
            case TaskCategory.Tech:
                Intellect++;
                break;

            case TaskCategory.Art:
            case TaskCategory.Chores:
            case TaskCategory.Hobbies:
                Dexterity++;
                break;

            case TaskCategory.Health:
            case TaskCategory.Family:
            case TaskCategory.SelfCare:
                Wisdom++;
                break;

            default:
                break;
        }
    }
}