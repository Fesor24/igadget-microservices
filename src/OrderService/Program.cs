using OrderService.Endpoints;
using OrderService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterApplicationServices(builder.Configuration);

builder.Services.RegisterAuthServices(builder.Configuration);

var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

OrderEndpointDefinition.RegisterEndpoints(app);

app.Run();
