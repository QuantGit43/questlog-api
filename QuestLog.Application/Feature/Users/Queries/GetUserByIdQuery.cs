using MediatR;
using QuestLog.Application.Dto;

namespace QuestLog.Application.Feature.Users.Queries;

public class GetUserByIdQuery : IRequest<UserDto>
{
    public Guid UserId { get; set; }
}