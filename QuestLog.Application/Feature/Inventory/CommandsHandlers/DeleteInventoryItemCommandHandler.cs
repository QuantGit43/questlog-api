using MediatR;
using QuestLog.Application.Feature.Inventory.Commands;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Inventory.CommandsHandlers;

public class DeleteInventoryItemCommandHandler: IRequestHandler<DeleteInventoryItemCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public DeleteInventoryItemCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task Handle(DeleteInventoryItemCommand request, CancellationToken cancellationToken)
    {
        var inventoryItem = await _unitOfWork.Inventory.GetByIdAsync(request.ItemId);
        if (inventoryItem == null)
        {
            throw new KeyNotFoundException($"Inventory item with ID {request.ItemId} not found.");
        }
        var avatar = await _unitOfWork.Avatars.GetByUserIdAsync(_userContext.UserId);
        if (inventoryItem.AvatarId != avatar?.Id && !_userContext.IsAdmin)
        {
            throw new UnauthorizedAccessException("Цей предмет вам не належить.");
        }
        if (inventoryItem.Quantity > 1)
        {
            inventoryItem.Quantity--;
            _unitOfWork.Inventory.Update(inventoryItem);
        }
        else
        {
            _unitOfWork.Inventory.Remove(inventoryItem);
        }
        
        await _unitOfWork.CompleteAsync();
    }
}