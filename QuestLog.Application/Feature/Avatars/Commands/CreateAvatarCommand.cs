using MediatR;

namespace QuestLog.Application.Feature.Avatars.Commands;

public record CreateAvatarCommand(int ClassId, string ClassName) : IRequest<Guid>;