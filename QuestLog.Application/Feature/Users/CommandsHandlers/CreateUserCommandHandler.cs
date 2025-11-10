using MediatR;
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
            throw new Exception("Користувач з таким email вже існує.");
        }

        if (await _unitOfWork.Users.GetByUsernameAsync(request.Username) != null)
        {
            throw new Exception("Користувач з таким нікнеймом вже існує.");
        }

        var hashedPasword = request.Password; //Тимчасова заглушка

        var avatar = new Avatar(request.AvatarName, request.AvatarClass);

        var user = new User(request.Username, request.Email, hashedPasword, avatar);
        
        await _unitOfWork.Users.AddAsync(user);

        await _unitOfWork.CompleteAsync();
        
        return user.Id;
    }
}