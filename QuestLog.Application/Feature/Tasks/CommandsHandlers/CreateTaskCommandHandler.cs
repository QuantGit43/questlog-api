using MediatR;
using QuestLog.Application.Feature.Tasks.Commands;
using QuestLog.Application.Feature.Users.Commands;
using QuestLog.Domain.Entities;
using QuestLog.Domain.Interfaces;
using Task = QuestLog.Domain.Entities.Task;

namespace QuestLog.Application.Feature.Tasks.CommandsHandlers;

public class CreateTaskCommandHandler: IRequestHandler<CreateTaskCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    
    public CreateTaskCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var currentAvatarId = _userContext.AvatarId;

        if (currentAvatarId == Guid.Empty)
        {
            throw new UnauthorizedAccessException("Користувач не має прив'язаного аватара або не авторизований.");
        }

        var avatarExists = await _unitOfWork.Avatars
            .AnyAsync(a => a.Id == currentAvatarId); 

        if (!avatarExists)
        {
            throw new KeyNotFoundException($"Аватар з ID {currentAvatarId} не знайдений у базі даних.");
        }

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new ArgumentException("Заголовок завдання не може бути пустим.");
        }

        var task = new Task(
            currentAvatarId, 
            request.Title,
            request.Type,
            request.XPReward,
            request.GoldReward,
            request.Description,
            request.DueDate
        );

        await _unitOfWork.Tasks.AddAsync(task);
        await _unitOfWork.CompleteAsync();

        return task.Id;
    }
}