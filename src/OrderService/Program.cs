using OrderService;
using OrderService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

app.ApplyMigrations();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

EndpointExtensions.RegisterApplicationEndpoints(app);

app.Run();
