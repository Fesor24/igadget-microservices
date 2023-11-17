using FluentAssertions;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Data;
using ProductService.IntegrationTests.Fixtures;
using ProductService.IntegrationTests.Utils;
using ProductService.Request;
using Shared.Contracts;
using System.Net.Http.Json;

namespace ProductService.IntegrationTests.Tests;

[Collection("Shared")]
public class ProductBusTests : IAsyncLifetime
{
    private readonly ProductSvcApplicationFactory _factory;
    private readonly HttpClient _httpClient;
    private readonly ITestHarness _testHarness;

    public ProductBusTests(ProductSvcApplicationFactory factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
        _testHarness = factory.Services.GetTestHarness();
    }

    [Fact]
    public async Task CreateProduct_WithValidData_PublishMessageToBus()
    {
        CreateProductRequest req = new()
        {
            ImageUrl = "random-img-url",
            Description = "random-description",
            Name = "Samsung",
            Price = 1900000m,
            BrandId = Guid.Parse("a9effcdf-90cd-488d-84e0-4f72d0a0cc16"),
            CategoryId = Guid.Parse("e8b83e8d-deff-4228-9c03-1828d233eb2e"),
            YearOfRelease = 2022
        };

        var response = await _httpClient.PostAsJsonAsync("api/products", req);

        response.EnsureSuccessStatusCode();

        var messagePublished = await _testHarness.Published.Any<ProductCreated>();

        messagePublished.Should().BeTrue();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync()
    {
        var scope = _factory.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

        Database.ReInitializeDb(context);

        return Task.CompletedTask;
    }
}
