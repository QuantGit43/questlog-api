using MediatR;

namespace QuestLog.Application.Feature.Inventory.Commands;

public class DeleteInventoryItemCommand: IRequest
{
    public Guid ItemId { get; set; }

}