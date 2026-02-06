using MediatR;
using QuestLog.Application.Feature.Item.Commands;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Item.CommandsHandlers;

public class BuyItemCommandHandler: IRequestHandler<BuyItemCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    
    public BuyItemCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<bool> Handle(BuyItemCommand request, CancellationToken cancellationToken)
    {
        var avatar = await _unitOfWork.Avatars.GetByIdAsync(_userContext.AvatarId);
        if (avatar == null)
        {
            throw new KeyNotFoundException("Аватар не знайдено.");
        }
        var item = await _unitOfWork.Items.GetByIdAsync(request.ItemId);
        if (item == null) 
        {
            throw new KeyNotFoundException($"Предмет з ID {request.ItemId} не знайдено.");
        }
        bool transactionSuccess = avatar.SpendGold(item.Price);
        if (!transactionSuccess)
        {
            throw new InvalidOperationException($"Not enough gold! You have {avatar.Gold}, need {item.Price}.");
        }
        var inventory = await _unitOfWork.Inventory
            .GetByAvatarAndItemAsync(avatar.Id, item.Id);

        if (inventory != null)
        {
            inventory.Quantity++;
            _unitOfWork.Inventory.Update(inventory);
        }
        else
        {
           var newItem = new Domain.Entities.Inventory
           {
                AvatarId = avatar.Id,
                ItemId = item.Id,
                Quantity = 1
              };
           await _unitOfWork.Inventory.AddAsync(newItem);
        }

        _unitOfWork.Avatars.Update(avatar);
        await _unitOfWork.CompleteAsync();

        return true;
    }
}