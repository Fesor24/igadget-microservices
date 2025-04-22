using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace OrderService.Extensions;

public static class AuthenticationExtensions
{
    public static void RegisterAuthServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(opt =>
            {
                //opt.Authority = config["IdentityServiceUrl"];
                opt.Authority = config["Authentication:Issuer"];
                opt.Audience = config["Authentication:Audience"];
                //opt.Audience = "orderapi";
                opt.RequireHttpsMetadata = false;

                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = config["Authentication:Audience"],
                    ValidateIssuer = true,
                    ValidIssuer = config["Authentication:Issuer"],
                    ValidTypes = new[] {"at+jwt"},
                };
            });

        services.AddAuthorization();
    }
}
