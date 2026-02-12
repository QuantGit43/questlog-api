namespace QuestLog.Domain.Entities;

public class Inventory
{
    public Guid Id { get; set; }
    public Guid AvatarId { get; set; }
    public Guid ItemId { get; set; }
    public int Quantity { get; set; }
    public virtual Item Item { get; set; }
    
    public Inventory()
    {
        Id = Guid.NewGuid();
    }
}