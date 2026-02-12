using MediatR;
using QuestLog.Application.Common.Extensions;
using QuestLog.Application.Common.Models;
using QuestLog.Application.Dto;
using QuestLog.Application.Feature.Item.Query;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Item.QueryHandler;

public class GetAllItemsQueryHandler: IRequestHandler<GetAllItemsQuery, PagedList<ItemDto>>
{
    private readonly IItemRepository _itemRepository;
    private readonly IUserContext _userContext;
    
    public GetAllItemsQueryHandler(IItemRepository itemRepository, IUserContext userContext)
    {
        _itemRepository = itemRepository;
        _userContext = userContext;
    }
    public async Task<PagedList<ItemDto>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
    {
        var query = _itemRepository.GetQueryable();
        
        query = query.OrderBy(u => u.Price);
    
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(t => t.Name.Contains(request.SearchTerm));
        } 
        
        var dtoQuery = query.Select(u => new ItemDto
        {
            Id = u.Id,
            Name = u.Name,
            Description = u.Description,
            Price = u.Price,
            Type = u.Type,
            Slot = u.Slot,
            EffectValue = u.EffectValue,
            AssetId = u.AssetId
        });

        return await dtoQuery.ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}