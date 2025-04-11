using System.Security.Claims;
using Duende.IdentityModel;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Services;

public class CustomProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomProfileService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);

        if(user is not null)
        {
            var claims = await _userManager.GetClaimsAsync(user);

            var userClaims = new List<Claim>
            {
                new Claim("username", user.UserName),
                new Claim(JwtClaimTypes.Email, user.Email)
            };

            context.IssuedClaims.AddRange(userClaims);
        }

    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        return Task.CompletedTask;
    }
}
