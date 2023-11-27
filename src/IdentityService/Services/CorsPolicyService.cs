using Duende.IdentityServer.Services;

namespace IdentityService.Services;

public class CorsPolicyService : ICorsPolicyService
{
    private readonly IConfiguration _config;

    public CorsPolicyService(IConfiguration config)
    {
        _config = config;
    }

    public Task<bool> IsOriginAllowedAsync(string origin)
    {
        string webClientOrigin = _config["WebHost"];

        return Task.FromResult(webClientOrigin == origin);
    }
}
