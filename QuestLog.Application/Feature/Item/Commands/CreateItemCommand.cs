using MediatR;
using QuestLog.Domain.Enums;

namespace QuestLog.Application.Feature.Item.Commands;

public class CreateItemCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public ItemType Type { get; set; }
    public EquipmentSlot Slot { get; set; }
    public int EffectValue { get; set; }
    public string AssetId { get; set; }
}