using MediatR;
using QuestLog.Application.Dto;

namespace QuestLog.Application.Feature.Users.Queries;

public class GetAvatarByIdQuery: IRequest<AvatarDto>
{
    public Guid AvatarId { get; set; }
}