using MediatR;
using Microsoft.Extensions.Logging;
using QuestLog.Application.Feature.Tasks.Commands;
using QuestLog.Application.Interfaces;
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

    public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _userContext.UserId;
        
        var avatar = await _unitOfWork.Avatars.GetByUserIdAsync(currentUserId);

        if (avatar == null)
        {
            _logger.LogError($"Avatar not found for UserID: {currentUserId}");
            throw new KeyNotFoundException($"Аватар для користувача {currentUserId} не знайдений.");
        }

        var difficulty = await _difficultyEvaluator.EvaluateAsync(request.Description ?? request.Title);

        var task = new Task(
            avatar.Id, 
            request.Title,
            request.Type,
            difficulty,
            request.Description,
            dueDate: request.DueDate
        );

        await _unitOfWork.Tasks.AddAsync(task);
        await _unitOfWork.CompleteAsync();

        return task.Id;
    }
}