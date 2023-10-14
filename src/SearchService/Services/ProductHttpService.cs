using SearchService.Entities;

namespace SearchService.Services;

public class ProductHttpService
{
    private readonly HttpClient _httpClient;

    public ProductHttpService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Product>> GetProducts()
    {
        string url = $"{_httpClient.BaseAddress}api/products";

        var response = await _httpClient.GetFromJsonAsync<List<Product>>(url);

        return response;
    }
}
