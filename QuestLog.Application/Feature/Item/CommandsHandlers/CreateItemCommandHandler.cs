using MediatR;
using QuestLog.Application.Feature.Item.Commands;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Item.CommandsHandlers;

public class CreateItemCommandHandler: IRequestHandler<CreateItemCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateItemCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var item = new Domain.Entities.Item
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Type = request.Type,
            Slot = request.Slot,
            EffectValue = request.EffectValue,
            MaleAssetId = request.MaleAssetId,
            FemaleAssetId = request.FemaleAssetId,
            RecommendedClass = request.RecommendedClass
        };
        await _unitOfWork.Items.AddAsync(item);
        await _unitOfWork.CompleteAsync();

        return item.Id;
    }
}