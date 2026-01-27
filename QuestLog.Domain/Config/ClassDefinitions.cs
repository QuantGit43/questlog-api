using QuestLog.Domain.Enums;

namespace QuestLog.Domain.Config;

public static class ClassDefinitions
{
    public static readonly Dictionary<AvatarClass, ClassStats> Stats = new()
    {
        { 
            AvatarClass.Warrior, 
            new ClassStats { MaxHp = 150, Strength = 10, Intellect = 3, Dexterity = 3, Wisdom = 3 } 
        },
        { 
            AvatarClass.Mage,    
            new ClassStats { MaxHp = 80,  Strength = 2,  Intellect = 10, Dexterity = 4, Wisdom = 5 } 
        },
        { 
            AvatarClass.Healer,  
            new ClassStats { MaxHp = 120, Strength = 3,  Intellect = 6,  Dexterity = 3, Wisdom = 10 } 
        },
        { 
            AvatarClass.Crafter, 
            new ClassStats { MaxHp = 100, Strength = 5,  Intellect = 5,  Dexterity = 10, Wisdom = 4 } 
        }
    };
}