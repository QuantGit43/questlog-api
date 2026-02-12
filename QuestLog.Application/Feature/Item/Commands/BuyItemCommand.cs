using MediatR;

namespace QuestLog.Application.Feature.Item.Commands;

public class BuyItemCommand : IRequest<bool>
{
    public Guid ItemId { get; set; }
}