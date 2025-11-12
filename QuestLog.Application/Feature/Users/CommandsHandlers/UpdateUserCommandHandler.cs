using MediatR;
using QuestLog.Application.Feature.Users.Commands;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Users.CommandsHandlers;

public class UpdateUserCommandHandler: IRequestHandler<UpdateUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        var exitingEmail = await _unitOfWork.Users.GetByEmailAsync(request.Email);
        if (exitingEmail != null && exitingEmail.Id != request.UserId)
        {
            throw new Exception("This Email is already occupied by another user.\n");
        }
        var existingUsername = await _unitOfWork.Users.GetByUsernameAsync(request.Username);
        if (existingUsername != null && existingUsername.Id != request.UserId)
        {
            throw new Exception("This Username is already occupied by another user.\n");
        }
        
        user.UpdateProfile(request.Username, request.Email );
        
        _unitOfWork.Users.Update(user);
        await _unitOfWork.CompleteAsync();
    }
}