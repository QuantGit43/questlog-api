using MediatR;
using QuestLog.Application.Feature.Avatars.Commands;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Avatars.CommandsHandlers;

public class UpdateAvatarCommandHandler: IRequestHandler<UpdateAvatarCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateAvatarCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateAvatarCommand request, CancellationToken cancellationToken)
    {
        var avatar = await _unitOfWork.Avatars.GetByIdAsync(request.AvatarId);
        if (avatar == null)
        {
            throw new KeyNotFoundException($"Аватар з ID {request.AvatarId} не знайдений.");
        }

        if (avatar.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("Ви не можете редагувати чужого аватара.");
        }
        avatar.ChangeName(request.NewName);
        
        _unitOfWork.Avatars.Update(avatar);
        await _unitOfWork.CompleteAsync();
    }
}