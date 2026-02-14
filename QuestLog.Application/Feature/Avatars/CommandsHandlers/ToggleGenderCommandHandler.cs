using MediatR;
using QuestLog.Application.Feature.Avatars.Commands;
using QuestLog.Domain.Entities;
using QuestLog.Domain.Enums;
using QuestLog.Domain.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace QuestLog.Application.Feature.Avatars.CommandsHandlers;

public class ToggleGenderCommandHandler : IRequestHandler<ToggleGenderCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public ToggleGenderCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<bool> Handle(ToggleGenderCommand request, CancellationToken cancellationToken)
    {
        var avatar = await _unitOfWork.Avatars.GetByIdAsync(_userContext.AvatarId);
        if (avatar == null) return false;

        avatar.ToggleGender();

        //Створюємо список того, що зараз одягнуто, щоб перевірити
        var equippedItems = new Dictionary<EquipmentSlot, Guid?>
        {
            { EquipmentSlot.Hair, avatar.EquippedHairId },
            { EquipmentSlot.Top, avatar.EquippedTopId },
            { EquipmentSlot.Bottom, avatar.EquippedBottomId },
            { EquipmentSlot.Gear, avatar.EquippedGearId }
        };

        //Проходимося по всьому одягу циклом
        foreach (var (slot, itemId) in equippedItems)
        {
            if (itemId.HasValue)
            {
                //Тут ми викликаємо маленьку локальну функцію 
                await ValidateAndUnequipIfNeeded(avatar, slot, itemId.Value);
            }
        }

        _unitOfWork.Avatars.Update(avatar);
        await _unitOfWork.CompleteAsync();
        return true;
    }

    private async Task ValidateAndUnequipIfNeeded(Avatar avatar, EquipmentSlot slot, Guid itemId)
    {
        var item = await _unitOfWork.Items.GetByIdAsync(itemId);
        
        //Якщо предмета не існує АБО він не підтримує нову стать
        if (item == null || !item.SupportsGender(avatar.VisualGender))
        {
            avatar.Unequip(slot);
        }
    }
}