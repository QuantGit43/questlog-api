using MediatR;
using QuestLog.Application.Dto;

namespace QuestLog.Application.Feature.Users.Queries;

public record  GetUserByIdQuery : IRequest<UserDto>
{
    public Guid UserId { get; set; }
}