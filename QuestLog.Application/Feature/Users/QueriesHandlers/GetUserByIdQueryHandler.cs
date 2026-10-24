using MediatR;
using QuestLog.Application.Dto;
using QuestLog.Application.Feature.Users.Queries;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Users.QueriesHandlers;

public class GetUserByIdQueryHandler: IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserRepository _repository;
    private readonly IUserContext _userContext;

    public GetUserByIdQueryHandler(IUserRepository repository, IUserContext userContext)
    {
        _repository = repository;
        _userContext = userContext;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.UserId != _userContext.UserId && !_userContext.IsAdmin)
        {
            throw new UnauthorizedAccessException("Ви не можете переглядати профіль іншого користувача.");
        }

        var user = await _repository.GetByIdAsync(request.UserId);

        if (user == null)
        {
            throw new KeyNotFoundException($"Користувача з ID {request.UserId} не знайдено.");
        }

        return new UserDto()
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            AvatarId = user.Avatar?.Id ?? Guid.Empty,
        };
    }
}