using MediatR;

namespace QuestLog.Application.Feature.Item.Commands;

public class DeleteItemCommand: IRequest
{
    public Guid ItemId { get; set; }
}