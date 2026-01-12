using MediatR;
using QuestLog.Domain.Enums;
using System.Text.Json.Serialization;

namespace QuestLog.Application.Feature.Tasks.Commands;

public class CreateTaskCommand : IRequest<Guid>
{
    // 1. Додаємо UserId. [JsonIgnore] означає, що Swagger і клієнт це поле не бачать.
    // Ми заповнюємо його в контролері з JWT токена.
    [JsonIgnore]
    public Guid UserId { get; set; }

    // 2. AvatarId тут більше не потрібен, бо хендлер сам знайде його через UserId.
    // Я прибрав його, щоб не плутатись.

    public string Title { get; set; }
    public string? Description { get; set; } // Може бути null
    public TaskType Type { get; set; }
    public DateTime? DueDate { get; set; }
}