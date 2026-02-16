using System.Text.Json.Serialization;
using MediatR;
using QuestLog.Domain.Enums;

namespace QuestLog.Application.Feature.Avatars.Commands;

public record CreateAvatarCommand([property: JsonPropertyName("class")] AvatarClass Class, 
    [property: JsonPropertyName("className")] string ClassName) : IRequest<Guid>;