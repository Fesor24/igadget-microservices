using OrderService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterApplicationServices(builder.Configuration);

builder.Services.RegisterAuthServices(builder.Configuration);

var app = builder.Build();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

EndpointExtensions.RegisterApplicationEndpoints(app);

app.Run();
