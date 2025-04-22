using ProductService.Extensions;
using ProductService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Commenting out authentication for prd service
// Auth is not necessary___did it for practice
//builder.Services.AddApplicationAuthentication(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.ConfigureExceptionHandler();

app.ApplyMigrations();

app.UseCors("CorsPolicy");

//app.UseAuthentication();

//app.UseAuthorization();

app.RegisterEndpoints();

app.MapGrpcService<GrpcProductService>();

app.Run();

public partial class Program { }
