using MediatR;
using QuestLog.Application.Dto;
using QuestLog.Application.Feature.Item.Query;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Item.QueryHandler;

public class GetItemByIdQueryHandler: IRequestHandler<GetItemByIdQuery, ItemDto>
{
    private readonly IItemRepository _itemRepository;
    
    public GetItemByIdQueryHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }
    
    public async Task<ItemDto> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _itemRepository.GetByIdAsync(request.Id);
        if (item == null)
        {
            throw new KeyNotFoundException($"Item with ID {request.Id} not found.");
        }
        
        return new ItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Price = item.Price,
            Type = item.Type,
            Slot = item.Slot,
            EffectValue = item.EffectValue,
            AssetId = item.AssetId
        };
    }
}