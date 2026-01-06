using FluentValidation;
using QuestLog.Application.Feature.Auth.Commands;

namespace QuestLog.Application.Feature.Auth.Validators;

public class LoginUserCommandValidator: AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Введіть Email.")
            .EmailAddress().WithMessage("Некоректний Email.");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Введіть пароль.");
    }
}