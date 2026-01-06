using System.Text.Json.Serialization;
using QuestLog.Domain.Enums;

namespace QuestLog.Application.Dto;

public class UserDto
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public Guid AvatarId { get; set; }
    [JsonIgnore]
    public string? AvatarName {get; set;}
    [JsonIgnore]
    public AvatarClass AvatarClass { get; set; }
    [JsonIgnore]
    public int AvatarLevel { get; set; }
}