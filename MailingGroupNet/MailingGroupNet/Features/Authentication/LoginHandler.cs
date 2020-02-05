using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.Dto;
using Model.Entity;

namespace MailingGroupNet.Features.Authentication
{
    public class AuthenticationModel : IRequest<ApiResult<List<Claim>>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }


    public class LoginHandler : IRequestHandler<AuthenticationModel, ApiResult<List<Claim>>>
    {
        private readonly UserManager<AppUser> _userManager;

        public LoginHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApiResult<List<Claim>>> Handle(AuthenticationModel request, CancellationToken cancellationToken)
        {
            AppUser user = await _userManager.Users.Where(x => x.UserName == request.UserName).FirstOrDefaultAsync(cancellationToken);

            if (user is null)
            {
                return ApiResult<List<Claim>>.Failed("Username or password is invalid", 400);
            }

            var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isValidPassword)
            {
                return ApiResult<List<Claim>>.Failed("Username or password is invalid", 400);
            }

            IList<Claim> claims = await _userManager.GetClaimsAsync(user);
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim("UserName", user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

            return ApiResult<List<Claim>>.Success(claims.ToList());
        }
    }
}
