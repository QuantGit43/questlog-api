using MediatR;
using QuestLog.Application.Feature.Avatars.Commands;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Avatars.CommandsHandlers;

public class UpdateAvatarCommandHandler: IRequestHandler<UpdateAvatarCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    
    public UpdateAvatarCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateAvatarCommand request, CancellationToken cancellationToken)
    {
        
        var avatar = await _unitOfWork.Avatars.GetByIdAsync(request.AvatarId);
        if (avatar == null)
        {
            throw new KeyNotFoundException($"Аватар з ID {request.AvatarId} не знайдений.");
        }
        if (request.AvatarId != _userContext.AvatarId)
        {
            throw new UnauthorizedAccessException("Ви не можете редагувати чужого аватара.");
        }
        avatar.UpdateDetails(request.Name, request.Class);
        
        _unitOfWork.Avatars.Update(avatar);
        await _unitOfWork.CompleteAsync();
    }
}