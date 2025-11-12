using MediatR;

namespace QuestLog.Application.Feature.Users.Commands;

public class UpdateAvatarCommand: IRequest
{
    public Guid AvatarId { get; set; }
    public string NewName { get; set; }
    public Guid UserId { get; set; }
}