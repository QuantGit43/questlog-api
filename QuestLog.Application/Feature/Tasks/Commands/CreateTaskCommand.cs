using System.Text.Json.Serialization;
using MediatR;
using QuestLog.Domain.Enums;

namespace QuestLog.Application.Feature.Tasks.Commands;

public class CreateTaskCommand : IRequest<Guid>
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public TaskType Type { get; set; }
    public TaskCategory Category { get; set; }
    public DateTime? DueDate { get; set; }
}