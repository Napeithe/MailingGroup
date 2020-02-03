using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MailingGroupNet.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Dto;

namespace MailingGroupNet.Features.Authentication
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class TokenAuthController : ControllerBase
    {
        private readonly TokenAuthConfiguration _configuration;
        private readonly IMediator _mediator;

        public TokenAuthController(TokenAuthConfiguration configuration, IMediator mediator)
        {
            _configuration = configuration;
            _mediator = mediator;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticateDto>> Authenticate([FromBody] AuthenticationModel model, CancellationToken token)
        {
            ApiResult<List<Claim>> result = await _mediator.Send(model, token);

            if (!result.IsSuccess)
            {
                return StatusCode(401);
            }

            var accessToken = CreateAccessToken(CreateJwtClaims(result.Data));

            return new AuthenticateDto()
            {
                AccessToken = accessToken,
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
            };
        }



        [HttpPost("register")]
        public async Task<ActionResult<ApiResult>> Register([FromBody] RegisterModel model, CancellationToken token)
        {
            var result = await _mediator.Send(model, token);
            if (!result.IsSuccess)
            {
                return StatusCode(400, result);
            }
            return result;
        }

        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(expiration ?? _configuration.Expiration),
                signingCredentials: _configuration.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private static List<Claim> CreateJwtClaims(IList<Claim> claims)
        {
            var claimsList = claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            claimsList.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            });

            return claimsList;
        }
    }
}
