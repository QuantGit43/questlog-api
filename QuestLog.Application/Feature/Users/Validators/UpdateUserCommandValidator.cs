using System.Runtime.InteropServices.ComTypes;
using FluentValidation;
using QuestLog.Application.Feature.Users.Commands;

namespace QuestLog.Application.Feature.Users.Validators;

public class UpdateUserCommandValidator: AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty).WithMessage("Не вказано ID користувача.");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Ім'я користувача не може бути порожнім.")
            .Length(3, 50).WithMessage("Ім'я має бути від 3 до 50 символів.");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email не може бути порожнім.")
            .EmailAddress().WithMessage("Некоректний формат Email.");
        
    }
}