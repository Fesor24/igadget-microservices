using ProductService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddApplicationAuthentication(builder.Configuration);

var app = builder.Build();

app.ConfigureExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.RegisterEndpoints();

app.Run();
