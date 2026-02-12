using MediatR;
using QuestLog.Application.Feature.Inventory.Commands;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Inventory.CommandsHandlers;

public class DeleteInventoryItemCommandHandler: IRequestHandler<DeleteInventoryItemCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteInventoryItemCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(DeleteInventoryItemCommand request, CancellationToken cancellationToken)
    {
        var inventoryItem = await _unitOfWork.Inventory.GetByIdAsync(request.ItemId);
        if (inventoryItem == null)
        {
            throw new KeyNotFoundException($"Inventory item with ID {request.ItemId} not found.");
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