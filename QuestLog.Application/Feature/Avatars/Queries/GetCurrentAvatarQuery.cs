using MediatR;
using QuestLog.Application.Dto;
using QuestLog.Domain.Enums;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Avatars.Queries;

// 1. Додаємо характеристики в DTO
public class AvatarProfileDto
{
    public string Username { get; set; }
    public long Gold { get; set; }
    public long Xp { get; set; }
    public int Hp { get; set; }
    public int MaxHp { get; set; } // Додав для зручності на фронтенді
    public int Level { get; set; }
    public AvatarClass Class { get; set; }
    
    // Нові поля для скрол-меню
    public int Strength { get; set; }
    public int Intellect { get; set; }
    public int Dexterity { get; set; }
    public int Wisdom { get; set; }
}

public class GetCurrentAvatarQuery : IRequest<AvatarProfileDto>
{
}

public class GetCurrentAvatarQueryHandler : IRequestHandler<GetCurrentAvatarQuery, AvatarProfileDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public GetCurrentAvatarQueryHandler(IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<AvatarProfileDto> Handle(GetCurrentAvatarQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
    
        var avatar = await _unitOfWork.Avatars.GetByUserIdAsync(userId);

        if (avatar == null) 
        {
            // Краще повертати null або кидати NotFoundException, 
            // але якщо залишаємо так, то варто ініціалізувати всі обов'язкові поля
            return new AvatarProfileDto { Username = "Unknown" }; 
        }

        // 2. Мапимо характеристики з Entity у DTO
        return new AvatarProfileDto
        {
            Username = avatar.User?.Username ?? avatar.Name ?? "Hero",
            Gold = avatar.Gold,
            Xp = avatar.XP,
            Hp = avatar.HP,
            MaxHp = avatar.MaxHP, // Беремо з сутності
            Level = avatar.Level, // Краще брати з avatar.Level, адже у вас там є своя логіка левелапів
            Class = avatar.Class,
            
            // Передаємо стати на фронтенд
            Strength = avatar.Strength,
            Intellect = avatar.Intellect,
            Dexterity = avatar.Dexterity,
            Wisdom = avatar.Wisdom
        };
    }
}