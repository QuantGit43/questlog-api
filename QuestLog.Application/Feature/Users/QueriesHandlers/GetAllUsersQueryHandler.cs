using MediatR;
using QuestLog.Application.Dto;
using QuestLog.Application.Feature.Users.Queries;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Users.QueriesHandlers;

public class GetAllUsersQueryHandler: IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
{
    private readonly IUserRepository _userRepository;
    
    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync();
        var userDtos = new List<UserDto>();
        foreach (var user in users)
        {
            var userWithAvatar = await _userRepository.GetByIdAsync(user.Id);
            if (userWithAvatar != null)
            {
                    userDtos.Add(new UserDto
                    {
                        Id = userWithAvatar.Id,
                        Username = userWithAvatar.Username,
                        Email = userWithAvatar.Email,
                        AvatarId = userWithAvatar.Avatar.Id,
                        AvatarName = userWithAvatar.Avatar.Name,
                        AvatarClass = userWithAvatar.Avatar.Class,
                        AvatarLevel = userWithAvatar.Avatar.Level
                    });
            }
        }

        return userDtos;
    }
}