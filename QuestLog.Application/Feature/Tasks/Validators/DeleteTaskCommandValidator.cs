using FluentValidation;
using QuestLog.Application.Feature.Tasks.Commands;

namespace QuestLog.Application.Feature.Tasks.Validators;

public class DeleteTaskCommandValidator : AbstractValidator<DeleteTaskCommand>
{
    public DeleteTaskCommandValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEqual(Guid.Empty).WithMessage("Не вказано ID завдання для видалення.");
    }
}