using MediatR;
using QuestLog.Application.Dto;

namespace QuestLog.Application.Feature.Item.Query;

public class GetItemByIdQuery: IRequest<ItemDto>
{
    public Guid Id { get; set; }
}