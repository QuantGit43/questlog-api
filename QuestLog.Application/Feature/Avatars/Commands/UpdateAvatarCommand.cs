using System.Text.Json.Serialization;
using MediatR;
using QuestLog.Domain.Enums;

namespace QuestLog.Application.Feature.Avatars.Commands;

public class UpdateAvatarCommand: IRequest
{
    public Guid AvatarId { get; set; }
    public string Name { get; set; }
    
    public AvatarClass Class { get; set; }
    
    [JsonIgnore]
    public Guid UserId { get; set; }
}