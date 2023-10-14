using Polly;
using Polly.Extensions.Http;
using SearchService.Endpoints;
using SearchService.Helper;
using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<ProductHttpService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["ProductServiceUrl"]);
    opt.DefaultRequestHeaders.Add("Accept", "application/json");
})
    .AddPolicyHandler(GetPolicy());

var app = builder.Build();

app.Lifetime.ApplicationStarted.Register(async () =>
{
    await DbInitializer.InitDb(app);
});

SearchEndpoint.Register(app);

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetPolicy() =>
    HttpPolicyExtensions
    .HandleTransientHttpError()
    //.OrResult(res => res.StatusCode == System.Net.HttpStatusCode.NotFound) // If we wanted to deal, for instance, a not found error
    .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));
