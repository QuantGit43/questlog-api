using MediatR;

namespace QuestLog.Application.Feature.Users.Commands;

public class UpdateTaskCommand: IRequest
{
    public Guid TaskId { get; set; }
    public Guid AvatarId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int XPReward { get; set; }
    public int GoldReward { get; set; }
}