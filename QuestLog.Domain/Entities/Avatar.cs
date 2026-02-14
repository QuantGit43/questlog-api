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
    
    public Guid? EquippedHairId { get; private set; }
    public Guid? EquippedTopId { get; private set; }
    public Guid? EquippedBottomId { get; private set; }
    public Guid? EquippedGearId { get; private set; }
    
    public Gender VisualGender { get; set; } = Gender.Male;
    
    public virtual ICollection<Inventory> Inventory { get; private set; } = new List<Inventory>();
    private Avatar() 
    {
        Tasks = new HashSet<Task>();
        Inventory = new HashSet<Inventory>();
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
        Inventory = new HashSet<Inventory>();
        
    }

    public void Equip(Item item)
    {
        if (item.Type == ItemType.Consumable)
        {
            throw new InvalidOperationException("Це не можна одягнути! Це треба використати (Use).");
        }
        switch (item.Slot)
        {
            case EquipmentSlot.Hair:
                EquippedHairId = item.Id;
                break;
            case EquipmentSlot.Top:
                EquippedTopId = item.Id;
                break;
            case EquipmentSlot.Bottom:
                EquippedBottomId = item.Id;
                break;
            case EquipmentSlot.Gear:
                EquippedGearId = item.Id;
                break;
            default:
                throw new InvalidOperationException("Невідомий слот для одягання.");
        }
    }

    public void Use(Item item)
    {
        if (item.Type == ItemType.Equipment)
        {
            throw new InvalidOperationException("Цей предмет не можна випити/з'їсти.");
        }
        if (item.Name.Contains("Health") || item.Name.Contains("HP"))
        {
            Heal(item.EffectValue); 
        }
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
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        long threshold = Level * 100;
        while (XP >= threshold)
        {
            XP -= threshold;
            Level++;
            MaxHP += 10;
            HP = MaxHP;
            threshold = Level * 100;
        }    }

    public void AddGold(int amount)
    {
        if (amount < 0) return;
        Gold += amount;
        CheckLevelUp();
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
    
    public bool SpendGold(int amount)
    {
        if (amount < 0 || amount > Gold) return false;

        Gold -= amount;
        return true;
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
        AddExperience(xp);
        AddGold(gold);
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
    public void ToggleGender()
    {
        VisualGender = VisualGender == Gender.Male ? Gender.Female : Gender.Male;
    }
    public void Unequip(EquipmentSlot slot)
    {
        switch (slot)
        {
            case EquipmentSlot.Hair: EquippedHairId = null; break;
            case EquipmentSlot.Top: EquippedTopId = null; break;
            case EquipmentSlot.Bottom: EquippedBottomId = null; break;
            case EquipmentSlot.Gear: EquippedGearId = null; break;
        }
    }
}