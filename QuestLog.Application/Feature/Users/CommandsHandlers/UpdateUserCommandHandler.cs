using MediatR;
using QuestLog.Application.Exceptions;
using QuestLog.Application.Feature.Users.Commands;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Users.CommandsHandlers;

public class UpdateUserCommandHandler: IRequestHandler<UpdateUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    
    public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);

        
        
        if (user == null)
        {
            throw new KeyNotFoundException($"Користувача з ID {request.UserId} не знайдено.");
        }
        
        if (user.Id != _userContext.UserId)
        {
            throw new UnauthorizedAccessException("Ви не можете редагувати профіль іншого користувача.");
        }
        var exitingEmail = await _unitOfWork.Users.GetByEmailAsync(request.Email);
        if (exitingEmail != null && exitingEmail.Id != request.UserId)
        {
            throw new ValidationException("Цей Email вже використовується іншим користувачем.");
        }
        var existingUsername = await _unitOfWork.Users.GetByUsernameAsync(request.Username);
        if (existingUsername != null && existingUsername.Id != request.UserId)
        {
            throw new ValidationException("Цей Username вже використовується іншим користувачем.");
        }
        
        user.UpdateProfile(request.Username, request.Email );
        
        _unitOfWork.Users.Update(user);
        await _unitOfWork.CompleteAsync();
    }
}