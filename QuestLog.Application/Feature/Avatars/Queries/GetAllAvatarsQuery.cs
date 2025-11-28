using MediatR;
using QuestLog.Application.Dto;

namespace QuestLog.Application.Feature.Avatars.Queries;

public class GetAllAvatarsQuery: IRequest<IEnumerable<AvatarDto>>
{
    
}