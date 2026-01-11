using FluentValidation;
using QuestLog.Application.Feature.Auth.Commands;

namespace QuestLog.Application.Feature.Auth.Validators;

public class RegisterUserCommandValidator: AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Ім'я користувача обов'язкове.")
            .MinimumLength(3).WithMessage("Ім'я має бути не менше 3 символів.")
            .MaximumLength(50).WithMessage("Ім'я занадто довге.");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email обов'язковий.")
            .EmailAddress().WithMessage("Некоректний формат Email.");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обов'язковий.")
            .MinimumLength(6).WithMessage("Пароль має містити мінімум 6 символів.")
            .Matches(@"[A-Z]").WithMessage("Пароль має містити хоча б одну велику літеру.")
            .Matches(@"[0-9]").WithMessage("Пароль має містити хоча б одну цифру.");
    }
}