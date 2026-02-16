using MediatR;
using QuestLog.Application.Exceptions;
using QuestLog.Application.Feature.Auth.Commands;
using QuestLog.Domain.Entities;
using QuestLog.Domain.Enums;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Auth.CommandsHandlers;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher; 
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    // IUserContext тут не потрібен, бо користувач ще не залогінений
    // private readonly IUserContext _userContext; 

    public RegisterUserHandler(
        IUserRepository userRepository, 
        IPasswordHasher passwordHasher, 
        IUnitOfWork unitOfWork,
        IJwtTokenGenerator jwtTokenGenerator
        /* IUserContext userContext */)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _jwtTokenGenerator = jwtTokenGenerator;
        // _userContext = userContext;
    }

    public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.IsEmailOrUsernameTakenAsync(request.Email, request.Username))
        {
            throw new CredentialsConflictException("Email or username already exists.");
        }
        
        var passwordHash = _passwordHasher.Hash(request.Password);

        // ❌ ВИДАЛЯЄМО ЦЕ: Ми не створюємо аватара при реєстрації!
        // var avatar = new Avatar(_userContext.UserId); 
        
        // 👇 Створюємо юзера БЕЗ аватара
        // (Вам може знадобитися оновити конструктор User, див. нижче)
        var user = new User(
            request.Username,
            request.Email,
            passwordHash
            // avatar <--- Прибираємо передачу аватара
        );
        
        var token = _jwtTokenGenerator.GenerateToken(user);
        
        bool isFirstUser = !(await _unitOfWork.Users.AnyAsync(u => true));
        if (isFirstUser)
        {
            user.PromoteToAdmin();
        }
        
        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponse
        {
            UserId = user.Id.ToString(),
            Username = user.Username,
            Token = token,
            Role = user.Role.ToString()
        };
    }
}