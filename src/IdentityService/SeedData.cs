using System.Security.Claims;
using Duende.IdentityModel;
using IdentityService.Data;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityService;

public static class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        
        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
        context.Database.Migrate();

        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (userMgr.Users.Any()) return;

        var fesor = userMgr.FindByNameAsync("fesor").Result;
        if (fesor == null)
        {
            fesor = new ApplicationUser
            {
                UserName = "fesor",
                Email = "FesorDev@email.com",
                EmailConfirmed = true,
            };
            var result = userMgr.CreateAsync(fesor, "Pass123$").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(fesor, new Claim[]{
                new Claim(JwtClaimTypes.Name, "Fesor Dev"),
                new Claim(JwtClaimTypes.GivenName, "Fesor"),
                new Claim(JwtClaimTypes.FamilyName, "Dev")
            }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            Log.Debug("fesor created");
        }
        else
        {
            Log.Debug("fesor already exists");
        }
    }
}
