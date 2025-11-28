using MediatR;
using QuestLog.Application.Feature.Tasks.Commands;
using QuestLog.Application.Feature.Users.Commands;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Tasks.CommandsHandlers;

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
            throw new KeyNotFoundException($"Завдання з ID {request.TaskId} не знайдено.");
        }
        if (task.OwnerAvatarId != request.AvatarId)
        {
            throw new UnauthorizedAccessException("Це завдання вам не належить.");
        }
        _unitOfWork.Tasks.Remove(task);
        await _unitOfWork.CompleteAsync();
    }
    
}