using MediatR;
using QuestLog.Application.Feature.Users.Commands;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Users.CommandsHandlers;

public class DeleteUserCommandHandler: IRequestHandler<DeleteUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    public DeleteUserCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
        
        if (user == null)
        {
            throw new KeyNotFoundException($"Користувача з ID {request.UserId} не знайдено.");
        }
        
        if (user.Id != _userContext.UserId && !_userContext.IsAdmin)
        {
            throw new UnauthorizedAccessException("Ви не можете видалити акаунт іншого користувача.");
        }
        
        var avatar = await _unitOfWork.Avatars.GetByIdAsync(user.AvatarId); 
        if (avatar != null) _unitOfWork.Avatars.Remove(avatar);
        _unitOfWork.Users.Remove(user);
        await _unitOfWork.CompleteAsync();
    }
}