using System.Security.Claims;

namespace GPMS.Helpers
{
    public static class TeamHelper
    {

        //internal static long GetUserIdFromToken(ClaimsPrincipal user)
        //{
        //    if (user.Identity?.IsAuthenticated != true)
        //        throw new Exception("User not authenticated");

        //    var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "sub");
        //    if (userIdClaim == null)
        //        throw new Exception("UserId not found in token");

        //    return long.Parse(userIdClaim.Value);
        //}
    }
}
