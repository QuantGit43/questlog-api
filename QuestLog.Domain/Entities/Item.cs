using QuestLog.Domain.Enums;

namespace QuestLog.Domain.Entities;

public class Item 
{
    public Guid Id { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    
    public string? MaleAssetId { get; set; } = string.Empty;   
    public string? FemaleAssetId { get; set; } = string.Empty;
    public ItemType Type { get; set; }
    public EquipmentSlot Slot { get; set; }
    
    public AvatarClass? RecommendedClass { get; set; }
    public int EffectValue { get; set; }
    
    public Item()
    {
        Id = Guid.NewGuid();
    }
    public bool SupportsGender(Gender gender)
    {
        return gender == Gender.Male 
            ? !string.IsNullOrEmpty(MaleAssetId) 
            : !string.IsNullOrEmpty(FemaleAssetId);
    }
}