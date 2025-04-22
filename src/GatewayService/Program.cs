using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        //options.Authority = builder.Configuration["IdentityServerUrl"];
        options.Authority = builder.Configuration["Authentication:Issuer"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            ValidateAudience = true,
            ValidateLifetime = true,
        };
        options.RequireHttpsMetadata = false;
        //options.Audience = "orderapi";
        options.Audience = builder.Configuration["Authentication:Audience"];
    });

builder.Services.AddCors(policy =>
{
    policy.AddPolicy("CorsPolicy", pol =>
    {
        pol.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});

var app = builder.Build();

app.UseCors("CorsPolicy");

app.MapReverseProxy();

app.UseAuthentication();

app.UseAuthorization();

app.Run();
