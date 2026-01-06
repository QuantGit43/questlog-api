using MediatR;
using QuestLog.Application.Exceptions;
using QuestLog.Application.Feature.Auth.Commands;
using QuestLog.Domain.Entities;
using QuestLog.Domain.Enums;
using QuestLog.Domain.Exeptions;
using QuestLog.Domain.Interfaces;
using QuestLog.Application.Exceptions;

namespace QuestLog.Application.Feature.Auth.CommandsHandlers;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher; 
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RegisterUserHandler(
        IUserRepository userRepository, 
        IPasswordHasher passwordHasher, 
        IUnitOfWork unitOfWork,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.IsEmailOrUsernameTakenAsync(request.Email, request.Username))
        {
            throw new CredentialsConflictException("Email or username already exists.");
        }

        var passwordHash = _passwordHasher.Hash(request.Password);

        var avatar = new Avatar(request.Username, AvatarClass.Warrior); // Warrior is default avatar
        var user = new User(
            request.Username,
            request.Email,
            passwordHash,
            avatar
        );
        
        var token = _jwtTokenGenerator.GenerateToken(user);

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponse
        {
            UserId = user.Id.ToString(),
            Username = user.Username,
            Token = token
        };
    }
}