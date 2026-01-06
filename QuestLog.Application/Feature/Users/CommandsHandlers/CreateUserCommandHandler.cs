using MediatR;
using QuestLog.Application.Exceptions;
using QuestLog.Application.Feature.Users.Commands;
using QuestLog.Domain.Entities;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Users.CommandsHandlers;

public class CreateUserCommandHandler: IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Users.GetByEmailAsync(request.Email) != null)
        {
            throw new ValidationException($"Користувач з email '{request.Email}' вже існує.");
        }

        if (await _unitOfWork.Users.GetByUsernameAsync(request.Username) != null)
        {
            throw new ValidationException($"Користувач з нікнеймом '{request.Username}' вже існує.");
        }

        var hashedPasword = request.Password; // Тимчасова заглушка

        var avatar = new Avatar(request.AvatarName, request.AvatarClass);
        
        await _unitOfWork.Avatars.AddAsync(avatar);

        var user = new User(request.Username, request.Email, hashedPasword, avatar);
    
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.CompleteAsync();
    
        return user.Id;
    }

}