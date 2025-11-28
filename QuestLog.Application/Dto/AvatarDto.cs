using QuestLog.Domain.Enums;

namespace QuestLog.Application.Dto;

public class AvatarDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? Name { get; set; }
    public AvatarClass Class { get; set; }
    public int Level { get; set; }
    public long XP { get; set; }
    public int HP { get; set; }
    public int MaxHP { get; set; }
    public int Gold { get; set; }
}