using OrderService.Endpoints;
using OrderService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();

OrderEndpointDefinition.RegisterEndpoints(app);

app.Run();
