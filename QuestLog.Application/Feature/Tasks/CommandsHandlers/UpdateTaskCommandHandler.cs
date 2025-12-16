using MediatR;
using QuestLog.Application.Feature.Tasks.Commands;
using QuestLog.Application.Feature.Users.Commands;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Tasks.CommandsHandlers;

public class UpdateTaskCommandHandler: IRequestHandler<UpdateTaskCommand>
{
private readonly IUnitOfWork _unitOfWork;

public UpdateTaskCommandHandler(IUnitOfWork unitOfWork)
{
    _unitOfWork = unitOfWork;
}

public async Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
{
    var quest = await _unitOfWork.Tasks.GetByIdAsync(request.TaskId);
    if (quest == null)
    {
        throw new KeyNotFoundException($"Завдання з ID {request.TaskId} не знайдено.");
    }

    if (quest.OwnerAvatarId != request.AvatarId)
    {
        throw new UnauthorizedAccessException("Це завдання вам не належить.");
    }
    quest.UpdateDetails(request.Title, request.Description, request.XPReward, request.GoldReward, request.IsCompleted);

    _unitOfWork.Tasks.Update(quest);
    await _unitOfWork.CompleteAsync();
}
}