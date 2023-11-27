using Duende.IdentityServer.Models;

namespace IdentityService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("productapi", "Product Api")
            {
                Scopes = new []{"productapi.read", "productapi.write"}
            },
            new ApiResource("orderapi", "Order Api")
            {
                Scopes = new[]{"orderapi.full"}
            },
            new ApiResource("web_client", "Web Client")
            {
                Scopes = new[]{"orderapi.full"}
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("productapi.read", "Product Api Read"),
            new ApiScope("productapi.write", "Product Api Write"),
            new ApiScope("orderapi.full", "Order Api Full Access")
        };

    public static IEnumerable<Client> Clients(IConfiguration config) =>
        new Client[]
        {
            // m2m client credentials flow client
            new Client
            {
                ClientId = "search_service",
                ClientName = "Search Service",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("search-secret".Sha256()) },
                AllowedScopes = { "productapi.read", "productapi.write" },
            },

            // To test order-svc on postman
            new Client
            {
                ClientId = "order_svc_postman",
                ClientName = "Order Svc Postman",
                ClientSecrets = {new Secret("order-svc".Sha256())},
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedScopes = {"openid", "profile", "orderapi.full"},
                AlwaysIncludeUserClaimsInIdToken = true,
                AccessTokenLifetime = 3600 * 24
            },
            new Client
            {
                ClientId = "web_client",
                ClientName = "Web Client",
                ClientSecrets = { new Secret("web-secret".Sha256())},
                AllowedScopes = {"openid", "profile", "orderapi.full"},
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = {$"{config["WebHost"]}/signin-oidc" },
                PostLogoutRedirectUris = {config["WebHost"]},
                FrontChannelLogoutUri = $"{config["WebHost"]}/signout-oidc",
                AllowOfflineAccess = true,
                RequirePkce = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                AccessTokenLifetime = 3600 * 24,
                IdentityTokenLifetime = 3600 * 24
            }
        };
}
