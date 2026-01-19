using MediatR;
using QuestLog.Domain.Enums;
using System.Text.Json.Serialization;

namespace QuestLog.Application.Feature.Tasks.Commands;

public class CreateTaskCommand : IRequest<Guid>
{


    public string Title { get; set; }
    public string? Description { get; set; } // Може бути null
    public TaskType Type { get; set; }
    public DateTime? DueDate { get; set; }
}