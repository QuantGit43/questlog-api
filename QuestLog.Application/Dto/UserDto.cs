using QuestLog.Domain.Enums;

namespace QuestLog.Application.Dto;

public class UserDto
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public Guid AvatarId { get; set; }
    public string? AvatarName {get; set;}
    public AvatarClass AvatarClass { get; set; }
    public int AvatarLevel { get; set; }
}