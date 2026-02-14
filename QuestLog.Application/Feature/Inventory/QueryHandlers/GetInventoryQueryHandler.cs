using MediatR;
using QuestLog.Application.Common.Extensions;
using QuestLog.Application.Common.Models;
using QuestLog.Application.Dto;
using QuestLog.Application.Feature.Inventory.Query;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Inventory.QueryHandlers;

public class GetInventoryQueryHandler: IRequestHandler<GetInventoryQuery, PagedList<InventoryDto>>
{
    private readonly IInventoryRepository _inventoryRepository;
    
    public GetInventoryQueryHandler(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }
    
    public async Task<PagedList<InventoryDto>> Handle(GetInventoryQuery request, CancellationToken cancellationToken)
    {
        var query = _inventoryRepository.GetQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(i => i.Item.Name.Contains(request.SearchTerm));
        }
        
        query = query.OrderBy(u => u.Quantity);

        var dtoQuery = query.Select(u => new InventoryDto
        {
            Id = u.Id,
            AvatarId = u.AvatarId,
            Quantity = u.Quantity,
            Items = new ItemDto()
            {
                Id = u.ItemId,
                Name = u.Item.Name,
                Description = u.Item.Description,
                Price = u.Item.Price,
                Type = u.Item.Type,
                Slot = u.Item.Slot,
                EffectValue = u.Item.EffectValue,
                MaleAssetId = u.Item.MaleAssetId,
                FemaleAssetId = u.Item.FemaleAssetId
            }
        });

        return await dtoQuery.ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}