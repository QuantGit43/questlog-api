using MediatR;
using QuestLog.Application.Feature.Tasks.Commands;
using QuestLog.Application.Feature.Users.Commands;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Tasks.CommandsHandlers;

public class UpdateTaskCommandHandler: IRequestHandler<UpdateTaskCommand>
{
private readonly IUnitOfWork _unitOfWork;
private readonly IUserContext _userContext;

    public UpdateTaskCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _unitOfWork.Tasks.GetByIdAsync(request.TaskId);
        if (task == null)
        {
            throw new KeyNotFoundException($"Завдання з ID {request.TaskId} не знайдено.");
        }
        if (task.AvatarId != _userContext.AvatarId && !_userContext.IsAdmin)
        {
            throw new UnauthorizedAccessException("Це завдання вам не належить.");
        }
        task.UpdateDetails(request.Title, request.Description, request.XPReward, request.GoldReward);
        
        if (request.IsCompleted && !task.IsCompleted)
        {
            task.Complete();
        }


        _unitOfWork.Tasks.Update(task);
        await _unitOfWork.CompleteAsync();
    }
}