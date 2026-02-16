using MediatR;
using QuestLog.Application.Dto;
using QuestLog.Application.Feature.Avatars.Queries;
using QuestLog.Domain.Enums;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Application.Feature.Avatars.QueriesHandlers;

public class GetAvatarAppearanceQueryHandler: IRequestHandler<GetAvatarAppearanceQuery, AvatarAppearanceDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    
    public GetAvatarAppearanceQueryHandler(IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }
    
    public async Task<AvatarAppearanceDto> Handle(GetAvatarAppearanceQuery request, CancellationToken cancellationToken)
    {
        var avatar = await _unitOfWork.Avatars.GetByUserIdAsync(_userContext.UserId);
        if (avatar == null)
        {
            throw new KeyNotFoundException($"Avatar for user ID {_userContext.UserId} not found.");
        }
        var top = avatar.EquippedTopId.HasValue ? await _unitOfWork.Items.GetByIdAsync(avatar.EquippedTopId.Value) : null;
        var hair = avatar.EquippedHairId.HasValue ? await _unitOfWork.Items.GetByIdAsync(avatar.EquippedHairId.Value) : null;
        var gear = avatar.EquippedGearId.HasValue ? await _unitOfWork.Items.GetByIdAsync(avatar.EquippedGearId.Value) : null;
        bool isMale = avatar.VisualGender == Gender.Male;
        
        return new AvatarAppearanceDto
        {
            Gender = avatar.VisualGender.ToString(),
            BodyAsset = isMale ? "m_body_base" : "f_body_base",
        
            TopAsset = isMale ? top?.MaleAssetId : top?.FemaleAssetId,
            HairAsset = isMale ? hair?.MaleAssetId : hair?.FemaleAssetId,
            GearAsset = isMale ? gear?.MaleAssetId : gear?.FemaleAssetId
            
        };
    }
}