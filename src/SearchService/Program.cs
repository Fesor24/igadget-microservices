using MassTransit;
using Polly;
using Polly.Extensions.Http;
using SearchService.Consumers;
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

builder.Services.AddMassTransit(opt =>
{
    opt.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));

    opt.AddConsumersFromNamespaceContaining<ProductCreatedConsumer>();

    opt.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", host =>
        {
            host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
            host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
        });

        // Applies to specific endpoint
        cfg.ReceiveEndpoint("search-product-created", opt =>
        {
            // Add retry in case any error arises when consuming...
            opt.UseMessageRetry(msg => msg.Interval(6, 6));

            // Configure the consumer it should apply to
            opt.ConfigureConsumer<ProductCreatedConsumer>(context);
        });

        // This applies to all endpoints
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
