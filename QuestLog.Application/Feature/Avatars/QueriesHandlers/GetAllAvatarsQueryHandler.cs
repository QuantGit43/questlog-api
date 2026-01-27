using MediatR;
using QuestLog.Application.Common.Extensions;
using QuestLog.Application.Common.Models;
using QuestLog.Application.Dto;
using QuestLog.Application.Feature.Avatars.Queries;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Avatars.QueriesHandlers;

public class GetAllAvatarsQueryHandler : IRequestHandler<GetAllAvatarsQuery, PagedList<AvatarDto>>
{
    private readonly IAvatarRepository _avatarRepository;

    public GetAllAvatarsQueryHandler(IAvatarRepository avatarRepository)
    {
        _avatarRepository = avatarRepository;
    }

    public async Task<PagedList<AvatarDto>> Handle(GetAllAvatarsQuery request, CancellationToken cancellationToken)
    {
        var query = _avatarRepository.GetQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(a => a.Name != null && a.Name.Contains(request.SearchTerm));
        }

        query = query.OrderByDescending(a => a.Level)
            .ThenBy(a => a.Name);

        var dtoQuery = query.Select(a => new AvatarDto
        {
            Id = a.Id,
            Name = a.Name,
            Class = a.Class,
            Level = a.Level,
            XP = a.XP,
            UserId = a.UserId,
            MaxHP = a.MaxHP,
            HP = a.HP,
            Gold = a.Gold,
            Strength = a.Strength,
            Intellect = a.Intellect,
            Dexterity = a.Dexterity,
            Wisdom = a.Wisdom
        });

        return await dtoQuery.ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}