using SearchService.Entities;
using SearchService.Response;
using SearchService.Settings;
using Shared.Exceptions;
using System.Net.Http.Headers;

namespace SearchService.Services;

public class ProductHttpService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly SearchSrvAuthConfiguration _authConfig = new();

    public ProductHttpService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
        config.GetSection(SearchSrvAuthConfiguration.CONFIGURATION).Bind(_authConfig);
    }

    public async Task<List<Product>> GetProducts()
    {
        string prdUrl = $"{_httpClient.BaseAddress}api/products";

        // Send request to id server to get token
        var formContent = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("client_id", _authConfig.ClientId),
            new KeyValuePair<string, string>("client_secret", _authConfig.ClientSecret),
            new KeyValuePair<string, string>("grant_type", _authConfig.GrantTypes),
            new KeyValuePair<string, string>("scope", _authConfig.Scope)
        };

        var content = new FormUrlEncodedContent(formContent);

        var idSrvRes = await _httpClient.PostAsync($"{ _config["IdentityServerUrl"]}/connect/token", content);

        IdSrvTokenResponse tokenResponse = default;

        if (idSrvRes.IsSuccessStatusCode)
        {
            tokenResponse = await idSrvRes.Content.ReadFromJsonAsync<IdSrvTokenResponse>();
        }
        else
        {
            var error = await idSrvRes.Content.ReadAsStreamAsync();

            var errorMessage = idSrvRes.ReasonPhrase;

            Console.WriteLine(new { error, errorMessage});
        }

        if (tokenResponse is null || string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
            throw new ApiNotAuthorizedException("Token not present yet");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

        var response = await _httpClient.GetFromJsonAsync<List<Product>>(prdUrl);

        return response;
    }
}
