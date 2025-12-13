using MediatR;
using QuestLog.Application.Feature.Auth.Commands;
using QuestLog.Domain.Entities;
using QuestLog.Domain.Enums;
using QuestLog.Domain.Exeptions;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Auth.CommandsHandlers;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Unit>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher; 
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserHandler(
        IUserRepository userRepository, 
        IPasswordHasher passwordHasher, 
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.GetByEmailAsync(request.Email) != null)
        {
            throw new InvalidCredentialsException("User with this email already exists.");
        }

        var passwordHash = _passwordHasher.Hash(request.Password);

        var avatar = new Avatar(request.Username, AvatarClass.Warrior); // Warrior is default avatar
        var user = new User(
            request.Username,
            request.Email,
            passwordHash,
            avatar
        );

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}