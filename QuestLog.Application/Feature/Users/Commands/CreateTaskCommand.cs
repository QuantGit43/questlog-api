using MediatR;
using QuestLog.Domain.Enums;

namespace QuestLog.Application.Feature.Users.Commands;

public class CreateTaskCommand: IRequest<Guid>
{
    public Guid AvatarId { get; set; }
    
    public string Title { get; set; }
    public string Description { get; set; }
    public TaskType Type { get; set; }
    public int XPReward { get; set; }
    public int GoldReward { get; set; }
    public DateTime? DueDate { get; set; }
}