using MediatR;
using QuestLog.Application.Feature.Users.Commands;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Users.CommandsHandlers;

public class DeleteTaskCommandHandler: IRequestHandler<DeleteTaskCommand>
{
    private IUnitOfWork _unitOfWork;
    
    public DeleteTaskCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _unitOfWork.Tasks.GetByIdAsync(request.TaskId);
        if (task == null)
        {
            throw new Exception("Task not found");
        }
        if (task.OwnerAvatarId != request.AvatarId)
        {
            throw new Exception("Not owner avatar");
        }
        _unitOfWork.Tasks.Remove(task);
        await _unitOfWork.CompleteAsync();
    }
    
}