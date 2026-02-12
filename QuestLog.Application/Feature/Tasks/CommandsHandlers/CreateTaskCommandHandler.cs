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
    private readonly ITaskAnalyser _taskAnalyzer;
    private readonly ILogger<CreateTaskCommandHandler> _logger;
    private readonly IUserContext _userContext;

    public CreateTaskCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext,
        ITaskAnalyser taskAnalyzer,
        ILogger<CreateTaskCommandHandler> logger)
    {
        _userContext = userContext;
        _unitOfWork = unitOfWork;
        _taskAnalyzer = taskAnalyzer;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken ct)
    {
        var currentUserId = _userContext.UserId;
        var avatar = await _unitOfWork.Avatars.GetByUserIdAsync(currentUserId);

        if (avatar == null) throw new KeyNotFoundException("Avatar not found.");
        // 1. AI визначає "Що це" (Категорія) і "Наскільки важко" (Складність)
        var textToAnalyze = !string.IsNullOrWhiteSpace(request.Description) ? request.Description : request.Title;
        var aiResult = await _taskAnalyzer.AnalyseAsync(textToAnalyze);
        
        // 2. C# визначає "Коли це здати" (Час) на основі складності
        DateTime dueDate = CalculateDueDate(aiResult.Difficulty);
        
        // 3. C# визначає нагороду
        var (xp, gold) = CalculateRewards(aiResult.Difficulty);
        
        var task = new Task(
            avatar.Id, 
            request.Title,
            request.Type,
            aiResult.Difficulty,
            aiResult.Category,
            request.Description,
            xp,
            gold,
            dueDate: dueDate
        );

        await _unitOfWork.Tasks.AddAsync(task);
        await _unitOfWork.CompleteAsync();

        return task.Id;
    }

    private (int xp, int gold) CalculateRewards(DifficultyLevel difficulty)
    {
        return difficulty switch
        {
            DifficultyLevel.Easy => (10, 5),
            DifficultyLevel.Medium => (30, 15),
            DifficultyLevel.Hard => (70, 35),
            _ => (10, 5)
        };
    }

    private DateTime CalculateDueDate(DifficultyLevel difficulty)
    {
        var random = new Random();
        DateTime now = DateTime.UtcNow;

        return difficulty switch
        {
            DifficultyLevel.Easy => now.AddHours(random.Next(12, 49)),
            
            DifficultyLevel.Medium => now.AddDays(random.Next(3, 8)),
            
            DifficultyLevel.Hard => now.AddDays(random.Next(14, 31)),

            _ => now.AddDays(1)
        };
    }
}