using IdentityService.Data;
using IdentityService.Models;
using IdentityService.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityService;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddCors(policy =>
        {
            policy.AddPolicy("CorsPolicy", opt =>
            {
                opt.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            });
        });

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                if (builder.Environment.IsEnvironment("Docker"))
                {
                    options.IssuerUri = "identity-svc";
                }

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                //options.EmitStaticAudienceClaim = true;
            })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiResources(Config.ApiResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients(builder.Configuration))
            .AddAspNetIdentity<ApplicationUser>()
            .AddProfileService<CustomProfileService>()
            .AddCorsPolicyService<CorsPolicyService>();

        //builder.Services.AddAuthentication();

        builder.Services.ConfigureApplicationCookie(opt =>
        {
            opt.Cookie.SameSite = SameSiteMode.Lax;
        });
        

        return builder.Build();
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    { 
        app.UseSerilogRequestLogging();
    
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.Use(async (ctx, next) =>
        {
            using var scope = app.Services.CreateScope();
            ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.MigrateAsync();

            await next(ctx);
        });

        app.UseCors("CorsPolicy");

        app.UseStaticFiles();
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();
        
        app.MapRazorPages()
            .RequireAuthorization();

        return app;
    }
}