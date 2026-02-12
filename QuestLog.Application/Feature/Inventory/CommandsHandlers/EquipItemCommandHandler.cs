using MediatR;
using QuestLog.Application.Feature.Inventory.Commands;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Inventory.CommandsHandlers;

public class EquipItemCommandHandler: IRequestHandler<EquipItemCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    
    public EquipItemCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(EquipItemCommand request, CancellationToken cancellationToken)
    {
        var avatar = await _unitOfWork.Avatars.GetByIdAsync(_userContext.AvatarId);

        if (avatar == null)
        {
            throw new ApplicationException("Avatar not found");
        }
        
        var inventory = await _unitOfWork.Inventory
            .GetByAvatarAndItemAsync(avatar.Id, request.ItemId);
        
        if(inventory == null)
        {
            throw new ApplicationException("You don't own this item!");
        }

        var item = inventory.Item;
        
        avatar.Equip(item);
        
        _unitOfWork.Avatars.Update(avatar);
        await _unitOfWork.CompleteAsync();
        
        return true;
    }
}