// В: QuestLog.Application/Features/Auth/LoginUserHandler.cs

using MediatR;
using QuestLog.Application.Feature.Auth.Commands;
using QuestLog.Domain.Exeptions;
using QuestLog.Domain.Interfaces;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginUserHandler(
        IUserRepository userRepository, 
        IPasswordHasher passwordHasher, 
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Знайти юзера в базі
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
        {
            throw new Exception("Invalid email or password.");
        }

        // 2. Перевірити пароль
        if (!_passwordHasher.Verify(user.PasswordHash, request.Password))
        {
            throw new InvalidCredentialsException("Invalid email or password.");
        }

        // 3. Згенерувати JWT токен
        var token = _jwtTokenGenerator.GenerateToken(user);

        // 4. Повернути DTO з даними
        return new AuthResponse
        {
            UserId = user.Id.ToString(),
            Username = user.Username,
            Token = token
        };
    }
}