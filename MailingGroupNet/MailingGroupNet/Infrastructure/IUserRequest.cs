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
        public static IUserRequest InjectUserId(this IUserRequest request, ClaimsPrincipal identity)
        {
            Claim idClaim = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (idClaim != null)
            {
                request.UserId = idClaim.Value;
            }

            return request;
        }
    }
}
