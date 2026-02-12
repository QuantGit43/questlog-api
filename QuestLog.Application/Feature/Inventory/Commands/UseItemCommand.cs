using MediatR;

namespace QuestLog.Application.Feature.Inventory.Commands;

public class UseItemCommand: IRequest<bool>
{
    public Guid ItemId { get; set; }
}