using MediatR;
using QuestLog.Application.Dto;

namespace QuestLog.Application.Feature.Users.Queries;

public class GetAllTasksQuery: IRequest<IEnumerable<TaskDto>>
{
    
}