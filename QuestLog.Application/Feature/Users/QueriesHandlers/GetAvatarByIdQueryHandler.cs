using MediatR;
using QuestLog.Application.Dto;
using QuestLog.Application.Feature.Users.Queries;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Users.QueriesHandlers;

public class GetAvatarByIdQueryHandler : IRequestHandler<GetAvatarByIdQuery, AvatarDto>
{
  private readonly IAvatarRepository _avatarRepository;

  public GetAvatarByIdQueryHandler(IAvatarRepository avatarRepository)
  {
    _avatarRepository = avatarRepository;
  }

  public async Task<AvatarDto> Handle(GetAvatarByIdQuery request, CancellationToken cancellationToken)
  {
    var avatar = await _avatarRepository.GetByIdAsync(request.AvatarId);
    if (avatar == null)
    {
      throw new System.Exception("Avatar not found");
    }

    return new AvatarDto
    {
      Id = avatar.Id,
      UserId = avatar.UserId,
      Name = avatar.Name,
      Class = avatar.Class,
      Level = avatar.Level,
      XP = avatar.XP,
      HP = avatar.HP,
      MaxHP = avatar.MaxHP,
      Gold = avatar.Gold,
    };
  }
  
}