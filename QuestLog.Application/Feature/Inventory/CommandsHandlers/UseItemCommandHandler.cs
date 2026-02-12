using MediatR;
using QuestLog.Application.Feature.Inventory.Commands;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Inventory.CommandsHandlers;

public class UseItemCommandHandler: IRequestHandler<UseItemCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    
    public UseItemCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }
    
    public async Task<bool> Handle(UseItemCommand request, CancellationToken cancellationToken)
    {
        var avatar = await _unitOfWork.Avatars.GetByUserIdAsync(_userContext.UserId);
        var inventory = await _unitOfWork.Inventory.GetByAvatarAndItemAsync(avatar.Id, request.ItemId);
        
        if (inventory == null || inventory.Quantity < 1) 
            throw new Exception("Зілля закінчились!");
        
        var item = await _unitOfWork.Items.GetByIdAsync(inventory.ItemId);

        avatar.Use(item);
        _unitOfWork.Avatars.Update(avatar);
        
        _unitOfWork.Inventory.Remove(inventory);
        await _unitOfWork.CompleteAsync();

        return true;
    }
}