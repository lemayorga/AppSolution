using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SG.Infrastructure.Auth.JwtAuthentication;
using SG.Infrastructure.Auth.Services;

namespace SG.Infrastructure.Auth;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureAuth(this IServiceCollection services, IConfiguration configuration) 
    {
        JwtOptions jwtOptions = new();
        configuration.GetSection("JwtOptions").Bind(jwtOptions);

        services.AddSingleton(jwtOptions);

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(opts =>
        {
            byte[] signingKeyBytes = Encoding.UTF8.GetBytes(jwtOptions.SigningKey);

            opts.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateActor = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
            };
        });

        services.AddScoped<IJwtBuilder, JwtBuilder>();

        services.AddTransient<IPrincipalCurrentUser,PrincipalCurrentUserService>(provider =>
             new PrincipalCurrentUserService(provider.GetService<IHttpContextAccessor>()!)
        );
        return services.AddAuthorization();
    }
}