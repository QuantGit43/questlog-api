using MediatR;
using QuestLog.Application.Feature.Users.Commands;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Users.CommandsHandlers;

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
        throw new Exception("Quest not found.\n");
    }

    if (quest.OwnerAvatarId != request.AvatarId)
    {
        throw new Exception("You cannot edit someone else's quest.\n");
    }
    quest.UpdateDetails(request.Title, request.Description, request.XPReward, request.GoldReward);

    _unitOfWork.Tasks.Update(quest);
    await _unitOfWork.CompleteAsync();
}
}