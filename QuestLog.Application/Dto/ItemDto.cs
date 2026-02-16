using QuestLog.Domain.Enums;

namespace QuestLog.Application.Dto;

public class ItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public ItemType Type { get; set; }
    public EquipmentSlot Slot { get; set; }
    public int? EffectValue { get; set; }
    public string? MaleAssetId { get; set; }
    public string? FemaleAssetId { get; set; }
}