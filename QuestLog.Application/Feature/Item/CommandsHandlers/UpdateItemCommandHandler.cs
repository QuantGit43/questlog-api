using MediatR;
using QuestLog.Application.Feature.Item.Commands;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Item.CommandsHandlers;

public class UpdateItemCommandHandler: IRequestHandler<UpdateItemCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateItemCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _unitOfWork.Items.GetByIdAsync(request.ItemId);
        if (item == null)
        {
            throw new ApplicationException("Item not found");
        }

        item.Name = request.Name;
        item.Description = request.Description;
        item.Price = request.Price;
        item.Type = request.Type;
        item.Slot = request.Slot;
        item.EffectValue = request.EffectValue;
        item.MaleAssetId = request.MaleAssetId;
        item.FemaleAssetId = request.FemaleAssetId;

        _unitOfWork.Items.Update(item);
        await _unitOfWork.CompleteAsync();
    }
}