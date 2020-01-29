using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.Dto;

namespace MailingGroupNet.Features.Authentication
{
    public class AuthenticationModel : IRequest<ApiResult<List<Claim>>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }


    public class Login : IRequestHandler<AuthenticationModel, ApiResult<List<Claim>>>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public Login(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApiResult<List<Claim>>> Handle(AuthenticationModel request, CancellationToken cancellationToken)
        {
            IdentityUser user = await _userManager.Users.Where(x => x.UserName == request.UserName).FirstOrDefaultAsync(cancellationToken);

            if (user is null)
            {
                return ApiResult<List<Claim>>.Failed("Username or password is invalid");
            }

            var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isValidPassword)
            {
                return ApiResult<List<Claim>>.Failed("Username or password is invalid");
            };

            IList<Claim> claims = await _userManager.GetClaimsAsync(user);
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserName));

            return ApiResult<List<Claim>>.Success(claims.ToList());
        }
    }
}
