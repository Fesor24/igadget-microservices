using Duende.IdentityServer.Models;
using IdentityModel;

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
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("productapi.read", "Product Api Read"),
            new ApiScope("productapi.write", "Product Api Write"),
            new ApiScope("orderapi.full", "Order Api Full Access")
        };

    public static IEnumerable<Client> Clients =>
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

            // for angular application
            new Client
            {
                ClientId = "order_service",
                ClientName = "Order Service",
                ClientSecrets = { new Secret("order-secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = { "http://localhost:4200/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:4200/signout-callback-oidc" },
                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "orderapi.full" }
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
            }
        };
}
