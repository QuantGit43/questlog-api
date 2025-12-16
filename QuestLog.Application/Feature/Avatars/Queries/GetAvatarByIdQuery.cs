using System.Text.Json.Serialization;
using MediatR;
using QuestLog.Application.Dto;

namespace QuestLog.Application.Feature.Avatars.Queries;

public class GetAvatarByIdQuery: IRequest<AvatarDto>
{
    [JsonIgnore]
    public Guid AvatarId { get; set; }
}