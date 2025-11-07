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

        var avatar = new Avatar
        {
            Id = Guid.NewGuid(),
            Name = request.AvatarName,
            Class = request.AvatarClass,
            Level = 1,
            XP = 0,
            HP = 100,
            MaxHP = 100,
            Gold = 0,
        };

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            HashedPassword = hashedPasword,
            Avatar = avatar
        };
        
        await _unitOfWork.Users.AddAsync(user);

        await _unitOfWork.CompleteAsync();
        
        return user.Id;
    }
}