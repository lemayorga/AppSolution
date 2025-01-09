using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SG.Infrastructure.Auth.JwtAuthentication;

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
        })
        .AddJwtBearer(opts =>
        {
            //convert the string signing key to byte array
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
        return services.AddAuthorization();
    }
}
