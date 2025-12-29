using FluentValidation;
using QuestLog.Application.Feature.Tasks.Commands;

namespace QuestLog.Application.Feature.Tasks.Validators;

public class UpdateTaskCommandValidator: AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEqual(Guid.Empty).WithMessage("Некоректний ID завдання.");
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Назва завдання обов'язкова")
            .MaximumLength(100).WithMessage("Назва завдання занадто довга");
        RuleFor(x => x.XPReward)
            .GreaterThanOrEqualTo(0).WithMessage("Винагорода за досвід не може бути від'ємною");
        RuleFor(x => x.GoldReward)
            .GreaterThanOrEqualTo(0).WithMessage("Винагорода за золото не може бути від'ємною");
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Опис завдання занадто довгий"); }
}
