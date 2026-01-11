using MediatR;
using QuestLog.Domain.Enums;
using System.Text.Json.Serialization; // 1. Додайте цей using

namespace QuestLog.Application.Feature.Tasks.Commands;

public class CreateTaskCommand : IRequest<Guid>
{
    [JsonIgnore]
    public Guid AvatarId { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public TaskType Type { get; set; }
    public DateTime? DueDate { get; set; }
}