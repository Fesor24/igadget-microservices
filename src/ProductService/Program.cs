using ProductService.Extensions;
using ProductService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

// Commenting out authentication for prd service
// Auth is not necessary___did it for practice
//builder.Services.AddApplicationAuthentication(builder.Configuration);

var app = builder.Build();

app.ConfigureExceptionHandler();

//app.UseAuthentication();

//app.UseAuthorization();

app.RegisterEndpoints();

app.MapGrpcService<GrpcProductService>();

app.Run();

public partial class Program { }
