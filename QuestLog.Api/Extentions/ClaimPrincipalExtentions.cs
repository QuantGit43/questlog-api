namespace QuestLog.Api.Extentions;

using System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetAvatarId(this ClaimsPrincipal user)
    {
        var claim = user.Claims.FirstOrDefault(c => 
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