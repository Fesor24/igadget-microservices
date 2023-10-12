using ProductService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

app.ConfigureExceptionHandler();

app.RegisterEndpoints();

app.Run();
