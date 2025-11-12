using MediatR;
using QuestLog.Application.Feature.Users.Commands;
using QuestLog.Domain.Interfaces;
using Task = QuestLog.Domain.Entities.Task;


namespace QuestLog.Application.Feature.Users.CommandsHandlers;

public class CreateTaskCommandHandler: IRequestHandler<CreateTaskCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateTaskCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var avatarExists = await _unitOfWork.Avatars
            .AnyAsync(a => a.Id == request.AvatarId);       
            if (!avatarExists)
            {
                throw new Exception("Avatar not found");

            }
        var task = new Task(
            request.AvatarId,
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