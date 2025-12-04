using FluentValidation;
using QuestLog.Application.Feature.Users.Commands;

namespace QuestLog.Application.Feature.Users.Validators;

public class CreateUserCommandValidator: AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Нікнейм обов'язковий")
            .MaximumLength(50).WithMessage("Нікнейм занадто довгий");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email обов'язковий")
            .EmailAddress().WithMessage("Невірний формат Email");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обов'язковий")
            .MinimumLength(6).WithMessage("Пароль має бути мінімум 6 символів");
        
        RuleFor(x => x.AvatarName)
            .NotEmpty().WithMessage("Ім'я аватара обов'язкове")
            .MaximumLength(50).WithMessage("Ім'я аватара занадто довге.");
    }
}