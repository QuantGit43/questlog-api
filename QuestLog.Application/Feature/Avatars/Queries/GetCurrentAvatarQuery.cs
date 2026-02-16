using MediatR;
using QuestLog.Application.Dto;
using QuestLog.Domain.Enums;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Avatars.Queries;

public class AvatarProfileDto
{
    public string Username { get; set; }
    public long Gold { get; set; }
    public long Xp { get; set; }
    public int Hp { get; set; }
    public int Level { get; set; }
    public AvatarClass Class { get; set; }
}

public class GetCurrentAvatarQuery : IRequest<AvatarProfileDto>
{
}

public class GetCurrentAvatarQueryHandler : IRequestHandler<GetCurrentAvatarQuery, AvatarProfileDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext; // Сервіс для отримання ID з токена

    public GetCurrentAvatarQueryHandler(IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<AvatarProfileDto> Handle(GetCurrentAvatarQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
    
        // ВАЖЛИВО: Переконайтеся, що ваш репозиторій робить .Include(a => a.User), 
        // якщо ви хочете взяти User.UserName. 
        // Або просто беріть avatar.Name, якщо це ім'я персонажа.
        var avatar = await _unitOfWork.Avatars.GetByUserIdAsync(userId);

        if (avatar == null) return new AvatarProfileDto { Username = "Unknown", /* ... */ };

        return new AvatarProfileDto
        {
            Username = avatar.User?.Username ?? avatar.Name ?? "Hero",
            Gold = avatar.Gold,
            Xp = avatar.XP,
            Hp = avatar.HP,
            Level = (int)(avatar.XP / 100) + 1,
            
            Class = avatar.Class 
        };
    }
}