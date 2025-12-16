using System.Text.Json.Serialization;
using MediatR;

namespace QuestLog.Application.Feature.Tasks.Commands;

public class UpdateTaskCommand: IRequest
{
    public Guid TaskId { get; set; }
    
    [JsonIgnore]
    public Guid AvatarId { get; set; }
    
    public string Title { get; set; }
    public string Description { get; set; }
    public int XPReward { get; set; }
    public int GoldReward { get; set; }
    
    public bool IsCompleted { get; set; }
}