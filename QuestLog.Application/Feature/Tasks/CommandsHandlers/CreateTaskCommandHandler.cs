using MediatR;
using Microsoft.Extensions.Logging;
using QuestLog.Application.Feature.Tasks.Commands;
using QuestLog.Application.Interfaces;
using QuestLog.Domain.Enums;
using QuestLog.Domain.Interfaces;
using Task = QuestLog.Domain.Entities.Task;

namespace QuestLog.Application.Feature.Tasks.CommandsHandlers;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITaskDifficultyEvaluator _difficultyEvaluator;
    private readonly ILogger<CreateTaskCommandHandler> _logger;
    private readonly IUserContext _userContext;

    public CreateTaskCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext,
        ITaskDifficultyEvaluator difficultyEvaluator,
        ILogger<CreateTaskCommandHandler> logger)
    {
        _userContext = userContext;
        _unitOfWork = unitOfWork;
        _difficultyEvaluator = difficultyEvaluator;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken ct)
    {
        var currentUserId = _userContext.UserId;
        var avatar = await _unitOfWork.Avatars.GetByUserIdAsync(currentUserId);

        if (avatar == null) throw new KeyNotFoundException("Avatar not found.");

        // 1. AI оцінює складність (фінальне рішення за сервером)
        var difficulty = await _difficultyEvaluator.EvaluateAsync(request.Description ?? request.Title);

        // 2. Розрахунок нагород (бізнес-логіка)
        var (xp, gold) = difficulty switch
        {
            DifficultyLevel.Easy => (10, 5),
            DifficultyLevel.Medium => (30, 15),
            DifficultyLevel.Hard => (70, 35),
            _ => (10, 5)
        };

        var task = new Task(
            avatar.Id, 
            request.Title,
            request.Type,
            difficulty,
            request.Description,
            xp,
            gold,
            dueDate: request.DueDate
        );

        await _unitOfWork.Tasks.AddAsync(task);
        await _unitOfWork.CompleteAsync();

        return task.Id;
    }
}