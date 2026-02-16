using MediatR;
using Microsoft.EntityFrameworkCore;
using QuestLog.Domain.Entities;
using QuestLog.Domain.Enums;
using QuestLog.Domain.Interfaces;
using QuestLog.Domain.Config;
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
        
        var avatar = new Avatar(userId, request.ClassName, request.Class);

        if (!ClassDefinitions.Stats.TryGetValue(request.Class, out var stats))
        {
            stats = new ClassStats { MaxHp = 100, Strength = 5, Intellect = 5, Dexterity = 5, Wisdom = 5 };
        }

        avatar.ChooseClass(
            request.ClassName,
            request.Class,
            stats.MaxHp,
            stats.Strength,
            stats.Intellect,
            stats.Dexterity,
            stats.Wisdom
        );

        await _avatarRepository.AddAsync(avatar);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return avatar.Id;
    }
}