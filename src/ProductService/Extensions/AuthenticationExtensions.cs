using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ProductService.Extensions;

public static class AuthenticationExtensions
{
    public static void AddApplicationAuthentication(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.Authority = config["IdentityServerUrl"];
                opt.Audience = "productapi";
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidTypes = new[] {"at+jwt"}
                };
                opt.RequireHttpsMetadata = false;
            });

        services.AddAuthorization();
    }
}
