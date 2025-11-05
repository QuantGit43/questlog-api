using QuestLog.Domain.Enums;

namespace QuestLog.Domain.Entities;

public class Avatar
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
    
    public string Name { get; set; }
    public AvatarClass Class { get; set; }
    public int Level { get; set; }
    public long XP { get; set; }
    public int HP { get; set; }
    public int MaxHP { get; set; }
    public int Gold { get; set; }
    
    public virtual ICollection<Task>  Tasks { get; set; } = new List<Task>();
}