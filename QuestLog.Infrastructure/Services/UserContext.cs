using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Infrastructure.Services;

public class UserContext: IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public UserContext(IHttpContextAccessor httpContextAccessor){
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            var id = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return id != null ? Guid.Parse(id) : Guid.Empty;
        }
    }

    public Guid AvatarId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => 
                c.Type == "avatarId" || 
                c.Type == "AvatarId" ||
                c.Type.EndsWith("/avatarId"));
                
            if (claim == null || !Guid.TryParse(claim.Value, out var avatarId))
            {
                return Guid.Empty;
            }
            
            return avatarId;
        }
    }
    
    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}