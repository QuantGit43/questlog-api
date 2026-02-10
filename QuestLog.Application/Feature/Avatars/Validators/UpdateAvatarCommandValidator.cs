using FluentValidation;
using QuestLog.Application.Feature.Avatars.Commands;

namespace QuestLog.Application.Feature.Avatars.Validators;

public class UpdateAvatarCommandValidator : AbstractValidator<UpdateAvatarCommand>
{
    public UpdateAvatarCommandValidator()
    {
        RuleFor(x => x.AvatarId)
            .NotEqual(Guid.Empty).WithMessage("Некоректний ID аватара.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ім'я аватара обов'язкове")
            .MaximumLength(50).WithMessage("Ім'я аватара занадто довге");
        RuleFor(x => x.Class)
            .IsInEnum().WithMessage("Некоректний клас аватара.");
    }
}