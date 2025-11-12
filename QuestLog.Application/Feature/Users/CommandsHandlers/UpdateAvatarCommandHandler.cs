using MediatR;
using QuestLog.Application.Feature.Users.Commands;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Users.CommandsHandlers;

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
            throw new Exception("Avatar not found");
        }

        if (avatar.UserId != request.UserId)
        {
            throw new Exception("You cannot change someone else's Avatar.");
        }
        avatar.ChangeName(request.NewName);
        
        _unitOfWork.Avatars.Update(avatar);
        await _unitOfWork.CompleteAsync();
    }
}