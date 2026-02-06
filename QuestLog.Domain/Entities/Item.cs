using QuestLog.Domain.Enums;

namespace QuestLog.Domain.Entities;

public class Item 
{
    public Guid Id { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public ItemType Type { get; set; }
    public EquipmentSlot Slot { get; set; }
    public int EffectValue { get; set; }
    public string AssetId { get; set; }
    
    public Item()
    {
        Id = Guid.NewGuid();
    }
}