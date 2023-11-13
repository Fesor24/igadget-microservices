using Microsoft.AspNetCore.Mvc.Testing;

namespace ProductService.IntegrationTests.Fixtures;
public class ProductSvcApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public Task InitializeAsync()
    {
        throw new NotImplementedException();
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        throw new NotImplementedException();
    }
}
