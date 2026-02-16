using System.Xml.XPath;
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

        // 1. Визначаємо Складність та Категорію
        DifficultyLevel difficulty;
        TaskCategory category;

        if (request.Difficulty.HasValue && request.Category.HasValue)
        {
            difficulty = request.Difficulty.Value;
            category = request.Category.Value;
        }
        else
        {
            var textToAnalyze = !string.IsNullOrWhiteSpace(request.Description) ? request.Description : request.Title;
            var aiResult = await _taskAnalyzer.AnalyseAsync(textToAnalyze);
            
            difficulty = request.Difficulty ?? aiResult.Difficulty;
            category = request.Category ?? aiResult.Category;
        }

        // 2. Визначаємо фінальну нагороду (з валідацією)
        var (finalXp, finalGold) = ValidateClientRewards(request, difficulty, currentUserId);

        // 3. Визначаємо Дату
        DateTime dueDate = request.DueDate ?? CalculateDueDate(difficulty);

        var task = new Task(
            avatar.Id, 
            request.Title,
            request.Type,
            difficulty,
            category,
            request.Description,
            xpReward: finalXp,    
            goldReward: finalGold,
            dueDate: dueDate
        );

        await _unitOfWork.Tasks.AddAsync(task);
        await _unitOfWork.CompleteAsync();

        return task.Id;
    }

    /// <summary>
    /// Перевіряє нагороду від клієнта. Якщо вона підозріло висока — повертає стандартну.
    /// </summary>
    private (int xp, int gold) ValidateClientRewards(CreateTaskCommand request, DifficultyLevel difficulty, Guid userId)
    {
        // Спочатку рахуємо наші "чесні" стандартні значення
        var (standardXp, standardGold) = CalculateRewards(difficulty);

        // Якщо клієнт нічого не прислав — повертаємо стандарт
        if (!request.XpReward.HasValue || !request.GoldReward.HasValue)
        {
            return (standardXp, standardGold);
        }

        var clientXp = request.XpReward.Value;
        var clientGold = request.GoldReward.Value;

        // Отримуємо ліміти
        var (maxXp, maxGold) = GetMaxAllowedRewards(difficulty);

        // ВАЛІДАЦІЯ: Перевіряємо на чітерство
        if (clientXp > maxXp || clientGold > maxGold)
        {
            // Скидаємо до стандарту, ігноруючи клієнта
            return (standardXp, standardGold);
        }

        // Якщо все ок — повертаємо значення клієнта
        return (clientXp, clientGold);
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
    
    /// <summary>
    ///  Limits is used to verify rewards
    /// </summary>
    private (int maxXp, int maxGold) GetMaxAllowedRewards(DifficultyLevel difficulty)
    {
        return difficulty switch // (xp, gold)
        {
            DifficultyLevel.Easy => (20, 10),
            DifficultyLevel.Medium => (50, 25),
            DifficultyLevel.Hard => (100, 50),
            _ => (20, 10)
        };
    }

    private DateTime CalculateDueDate(DifficultyLevel difficulty)
    {
        var random = new Random();
        DateTime now = DateTime.UtcNow;

        return difficulty switch
        {
            DifficultyLevel.Easy => now.AddHours(24),
            DifficultyLevel.Medium => now.AddDays(7),
            DifficultyLevel.Hard => now.AddDays(14),
            _ => now.AddDays(1)
        };
    } 
}