using MediatR;
using QuestLog.Application.Common.Extensions; 
using QuestLog.Application.Common.Models;    
using QuestLog.Application.Dto;
using QuestLog.Application.Feature.Users.Queries;
using QuestLog.Domain.Enums; 
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Users.QueriesHandlers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PagedList<UserDto>>
{
    private readonly IUserRepository _userRepository; 

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PagedList<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _userRepository.GetQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.ToLower();
            query = query.Where(u => 
                u.Email.ToLower().Contains(term) || 
                u.Username.ToLower().Contains(term));
        }
        
        query = query.OrderBy(u => u.Username);

        var dtoQuery = query.Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            AvatarId = u.Avatar != null ? u.Avatar.Id : Guid.Empty 
        });

        return await dtoQuery.ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}