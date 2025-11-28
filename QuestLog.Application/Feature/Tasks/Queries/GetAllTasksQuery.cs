using MediatR;
using QuestLog.Application.Dto;

namespace QuestLog.Application.Feature.Tasks.Queries;

public class GetAllTasksQuery: IRequest<IEnumerable<TaskDto>>
{
    
}