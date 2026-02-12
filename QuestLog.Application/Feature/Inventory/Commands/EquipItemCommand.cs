using MediatR;

namespace QuestLog.Application.Feature.Inventory.Commands;

public class EquipItemCommand: IRequest<bool>
{
    public Guid ItemId { get; set; }
}