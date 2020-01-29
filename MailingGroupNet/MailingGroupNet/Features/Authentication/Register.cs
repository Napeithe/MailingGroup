using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Model.Dto;

namespace MailingGroupNet.Features.Authentication
{
    public class RegisterModel : IRequest<ApiResult>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }

    }

    public class RegisterHandler : IRequestHandler<RegisterModel, ApiResult>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public RegisterHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApiResult> Handle(RegisterModel request, CancellationToken cancellationToken)
        {
            if (!request.Password.Equals(request.RePassword))
            {
                return ApiResult.Failed("The passwords are not equal");
            }

            IdentityUser identityUser = new IdentityUser(request.UserName);
            identityUser.Email = request.Email;

            IdentityResult identityResult = await _userManager.CreateAsync(identityUser, request.Password);

            if (!identityResult.Succeeded)
            {
                return ApiResult.Failed(identityResult.Errors.FirstOrDefault()?.Description ?? "Invalid login");
            }

            return ApiResult.Success();
        }
    }
}
