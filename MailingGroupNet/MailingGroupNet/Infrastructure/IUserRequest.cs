using System.Linq;
using System.Security.Claims;

namespace MailingGroupNet.Infrastructure
{
    public interface IUserRequest
    {
        string UserId { get; set; }
    }

    public static class UserIdInjectorExtension
    {
        public static void InjectUserId(this IUserRequest request, ClaimsIdentity identity)
        {
            Claim idClaim = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (idClaim != null)
            {
                request.UserId = idClaim.Value;
            }
        }
    }
}
