using MediatR;
using QuestLog.Application.Feature.Item.Commands;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Item.CommandsHandlers;

public class DeleteItemCommandHandler: IRequestHandler<DeleteItemCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteItemCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _unitOfWork.Items.GetByIdAsync(request.ItemId);
        if (item == null)
        {
            throw new KeyNotFoundException($"Item with ID {request.ItemId} not found.");
        }
        
        _unitOfWork.Items.Remove(item);
        await _unitOfWork.CompleteAsync();
    }
}