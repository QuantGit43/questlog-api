using MediatR;
using Microsoft.EntityFrameworkCore;
using QuestLog.Domain.Entities;
using QuestLog.Domain.Enums;
using QuestLog.Domain.Interfaces;
using QuestLog.Application.Feature.Avatars.Commands;

namespace QuestLog.Application.Feature.Avatars.CommandsHandlers;

public class CreateAvatarCommandHandler : IRequestHandler<CreateAvatarCommand, Guid>
{
    private readonly IRepository<Avatar> _avatarRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAvatarCommandHandler(
        IRepository<Avatar> avatarRepository, 
        IUserContext userContext, 
        IUnitOfWork unitOfWork)
    {
        _avatarRepository = avatarRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateAvatarCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        
        var existingAvatar = await _unitOfWork.Avatars.GetByUserIdAsync(userId);

        if (existingAvatar != null)
        {
            return existingAvatar.Id;
        }
        
        var avatar = new Avatar(userId);

        switch (request.ClassId)
        {
            case 1: 
                avatar.ChooseClass("The Healer", AvatarClass.Healer, 80, 2, 4, 3, 8);
                break;
            case 2: 
                avatar.ChooseClass("The Warrior", AvatarClass.Warrior, 120, 8, 2, 4, 2);
                break;
            case 3: 
                avatar.ChooseClass("The Crafter", AvatarClass.Crafter, 100, 3, 5, 8, 3);
                break;
            case 4: 
                avatar.ChooseClass("The Mage", AvatarClass.Mage, 60, 1, 9, 3, 5);
                break;
            default:
                avatar.ChooseClass("Adventurer", AvatarClass.Warrior, 100, 5, 5, 5, 5);
                break;
        }

        await _avatarRepository.AddAsync(avatar);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return avatar.Id;
    }
}