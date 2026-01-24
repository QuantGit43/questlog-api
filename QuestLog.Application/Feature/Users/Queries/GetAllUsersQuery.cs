using MediatR;
using QuestLog.Application.Common.Models;
using QuestLog.Application.Dto;
using QuestLog.Domain.Enums;

namespace QuestLog.Application.Feature.Users.Queries;

public class GetAllUsersQuery: IRequest<PagedList<UserDto>>
{
    public string? SearchTerm { get; set; } 
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}