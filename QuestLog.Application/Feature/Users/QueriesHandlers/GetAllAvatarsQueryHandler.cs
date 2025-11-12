using MediatR;
using QuestLog.Application.Dto;
using QuestLog.Application.Feature.Users.Queries;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Users.QueriesHandlers;

public class GetAllAvatarsQueryHandler: IRequestHandler<GetAllAvatarsQuery, IEnumerable<AvatarDto>>
{
    private readonly IAvatarRepository _avatarRepository;
    
    public GetAllAvatarsQueryHandler(IAvatarRepository avatarRepository)
    {
        _avatarRepository = avatarRepository;
    }

    public async Task<IEnumerable<AvatarDto>> Handle(GetAllAvatarsQuery request, CancellationToken cancellationToken)
    {
        var avatars = await _avatarRepository.GetAllAsync();
        return avatars.Select(a => new AvatarDto
        {
            Id = a.Id,
            UserId = a.UserId,
            Name = a.Name,
            Class = a.Class,
            Level = a.Level,
            XP = a.XP,
            HP = a.HP,
            MaxHP = a.MaxHP,
            Gold = a.Gold
        });
    }
}