using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.Authority = builder.Configuration["IdentityServerUrl"];
//        options.RequireHttpsMetadata = false;
//        options.Audience = "productapi";
//    });

var app = builder.Build();

app.MapReverseProxy();

//app.UseAuthentication();

//app.UseAuthorization();

app.Run();
