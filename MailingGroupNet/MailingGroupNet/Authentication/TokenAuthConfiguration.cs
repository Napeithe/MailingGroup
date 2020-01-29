using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace MailingGroupNet.Authentication
{
    public class TokenAuthConfiguration
    {
        public SymmetricSecurityKey SecurityKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public SigningCredentials SigningCredentials { get; set; }

        public TimeSpan Expiration { get; set; }
    }

    public static class TokenAuthConfigurationExtension
    {
        public static void RegisterTokenAuth(this IServiceCollection service)
        {
            service.AddSingleton<TokenAuthConfiguration>();
        }

        public static void UseTokenAuthConfiguration(this IApplicationBuilder app, IConfiguration configuration)
        {
            IServiceProvider serviceProvider = app.ApplicationServices;
            TokenAuthConfiguration tokenAuthConfiguration = serviceProvider.GetService<TokenAuthConfiguration>();
            tokenAuthConfiguration.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfiguration.Issuer = configuration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfiguration.Audience = configuration["Authentication:JwtBearer:Audience"];
            tokenAuthConfiguration.SigningCredentials = new SigningCredentials(tokenAuthConfiguration.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfiguration.Expiration = TimeSpan.FromDays(1);
        }
    }
}
