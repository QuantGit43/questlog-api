using MediatR;
using QuestLog.Application.Common.Extensions;
using QuestLog.Application.Common.Models;
using QuestLog.Application.Dto;
using QuestLog.Application.Feature.Item.Query;
using QuestLog.Domain.Enums;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Item.QueryHandler;

public class GetShopItemQueryHandler: IRequestHandler<GetShopItemQuery, PagedList<ShopItemDto>>
{
    private readonly IItemRepository _itemRepository;
    private readonly IAvatarRepository _avatarRepository;
    private readonly IUserContext _userContext;
    
    public GetShopItemQueryHandler(IItemRepository itemRepository, IUserContext userContext, IAvatarRepository avatarRepository)
    {
        _itemRepository = itemRepository;
        _userContext = userContext;
        _avatarRepository = avatarRepository;
    }

    public async Task<PagedList<ShopItemDto>> Handle(GetShopItemQuery request, CancellationToken cancellationToken)
    {
        var avatarId = _userContext.AvatarId;
        if (avatarId == Guid.Empty) throw new UnauthorizedAccessException();
        var avatar = await _avatarRepository.GetByIdAsync(avatarId);
        if (avatar == null) throw new KeyNotFoundException("Avatar not found");
        bool isMale = avatar.VisualGender == Gender.Male;
        var avatarClass = avatar.Class;
        var query = _itemRepository.GetQueryable();
        
        query = query.Where(i => isMale ? i.MaleAssetId != null : i.FemaleAssetId != null);
        query = query.Where(i => i.RecommendedClass == null || i.RecommendedClass == avatarClass);
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(t => t.Name.Contains(request.SearchTerm));
        }
        query = query.OrderBy(u => u.Price);

        var dtoQuery = query.Select(u => new ShopItemDto
        {
            Id = u.Id,
            Name = u.Name,
            Price = u.Price,
            Slot = u.Slot.ToString(),
            AssetId = isMale ? u.MaleAssetId : u.FemaleAssetId,
            IsRecommended = u.RecommendedClass == null || u.RecommendedClass == avatarClass,
            Class = u.RecommendedClass.HasValue ? u.RecommendedClass.ToString() : "Any"
        });

        return await dtoQuery.ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}
    