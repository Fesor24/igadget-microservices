using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Data;
using ProductService.IntegrationTests.Fixtures;
using ProductService.IntegrationTests.Utils;
using ProductService.Request;
using ProductService.Response;
using System.Net.Http.Json;

namespace ProductService.IntegrationTests.EndpointTests;
public class ProductEndpointTest : IClassFixture<ProductSvcApplicationFactory>, IAsyncLifetime
{
    private readonly ProductSvcApplicationFactory _appFactory;

    private readonly HttpClient _httpClient;

    private const string _validProductId = "3e381e10-4be5-4406-ba23-3bd323b70710";

    private const string _brandId = "a9effcdf-90cd-488d-84e0-4f72d0a0cc16";

    private const string _categoryId = "e8b83e8d-deff-4228-9c03-1828d233eb2e";

    public ProductEndpointTest(ProductSvcApplicationFactory appFactory)
    {
        _appFactory = appFactory;
        _httpClient = appFactory.CreateClient();
    }

    [Fact]
    public async Task GetProducts_ShouldReturn_ProductList()
    {
        var response = await _httpClient.GetFromJsonAsync<List<GetProductResponse>>("api/products");

        using var _ = new AssertionScope();

        response.Should().NotBeNull()
            .And.ContainItemsAssignableTo<GetProductResponse>();

        response.Should().NotBeEmpty()
            .And.HaveCount(2);
    }

    [Fact]
    public async Task GetProductById_WithBaddGuid_ShouldReturn400StatusCode()
    {
        var response = await _httpClient.GetAsync("api/products/badguid");

        response.Should().HaveStatusCode(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetProductById_WithInvalidId_ShouldReturn404StatusCode()
    {
        var response = await _httpClient.GetAsync("api/products/29D70302-AE3F-425F-BEDF-03074847F3D1");

        response.Should().HaveStatusCode(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetProductById_WithValidId_ShouldReturnProduct()
    {
        var response = await _httpClient.GetAsync($"api/products/{_validProductId}");

        response.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);

        var product = await response.Content.ReadFromJsonAsync<GetProductResponse>();

        using var _ = new AssertionScope();

        product.Should().NotBeNull();

        product.Should().BeOfType<GetProductResponse>();
    }

    [Fact]
    public async Task CreateProduct_WithBadData_ShouldReturn422UnprocessableEntity()
    {
        var createProductRequest = ProductCreate();

        createProductRequest.Name = string.Empty;

        var response = await _httpClient.PostAsync($"api/products", JsonContent.Create(createProductRequest));

        response.Should().HaveStatusCode(System.Net.HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task CreateProduct_WithValidData_ShouldReturn201Created()
    {
        var response = await _httpClient.PostAsync($"api/products", JsonContent.Create(ProductCreate()));

        response.EnsureSuccessStatusCode();

        response.Should().HaveStatusCode(System.Net.HttpStatusCode.Created);

        var createdProduct = await response.Content.ReadFromJsonAsync<GetProductResponse>();

        createdProduct.Should().NotBeNull();

        createdProduct.Should().BeOfType<GetProductResponse>();
    }

    [Fact]
    public async Task UpdateProduct_WithInvalidId_ShouldReturn404()
    {
        UpdateProductRequest req = new()
        {
            Name = "Xaomi",
            Price = 1920293m,
            Description = "Update xaoimi",
            ImageUrl = "updated-img-url"
        };

        var response = await _httpClient.PutAsync("api/products/B69E5408-475F-46A7-A8B4-3FF062A962D1", 
            JsonContent.Create(req));

        response.Should().HaveStatusCode(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateProduct_WithValidId_ShouldReturn204NoContent()
    {
        UpdateProductRequest req = new()
        {
            Name = "Xaomi",
            Price = 1920293m,
            Description = "Update xaoimi",
            ImageUrl = "updated-img-url"
        };

        var response = await _httpClient.PutAsync($"api/products/{_validProductId}",
            JsonContent.Create(req));

        response.EnsureSuccessStatusCode();

        response.Should().HaveStatusCode(System.Net.HttpStatusCode.NoContent);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync()
    {
        var scope = _appFactory.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

        Database.ReInitializeDb(db);

        return Task.CompletedTask;
    }

    private CreateProductRequest ProductCreate()
    {
        return new CreateProductRequest
        {
            Price = 500.00m,
            Name = "Xaoimi",
            Description = "Xaoimi phone",
            ImageUrl = "random-img",
            BrandId = Guid.Parse(_brandId),
            CategoryId = Guid.Parse(_categoryId),
            YearOfRelease = 2015
        };
    }
}
