using MediatR;
using QuestLog.Application.Dto;
using QuestLog.Application.Feature.Tasks.Commands;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Tasks.CommandsHandlers;

public class CompleteTaskCommandHandler: IRequestHandler<CompleteTaskCommand, TaskCompletionDto>
{
    public readonly IUnitOfWork _unitOfWork;
    public readonly IUserContext _userContext;

    public CompleteTaskCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }
    public async Task<TaskCompletionDto> Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _unitOfWork.Tasks.GetByIdAsync(request.TaskId);
        if (task == null)
        {
            throw new KeyNotFoundException($"Завдання з ID {request.TaskId} не знайдено.");
        }
        var avatar = await _unitOfWork.Avatars.GetByIdAsync(task.AvatarId);
        if (avatar != null && avatar.Id != _userContext.AvatarId && !_userContext.IsAdmin)
        {
            throw new UnauthorizedAccessException("Ви не можете виконати чуже завдання.");
        }
        
        var (earnedXp, earnedGold) = task.Complete(avatar);
        
       _unitOfWork.Tasks.Update(task);
       _unitOfWork.Avatars.Update(avatar);
    
       await _unitOfWork.CompleteAsync();

       return new TaskCompletionDto(earnedXp, earnedGold);       
    }
}