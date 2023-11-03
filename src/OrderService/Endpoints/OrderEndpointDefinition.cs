using OrderService.Services.Contracts;

namespace OrderService.Endpoints;

public class OrderEndpointDefinition
{
    public static void RegisterEndpoints(WebApplication app)
    {
        var order = app.MapGroup("api/order");

        // Endpoint to test the grpc cart client
        order.MapGet("/grpc-cart-test/{id}", (string id, IGrpcClient cartClient) =>
        {
            var cart = cartClient.GetShoppingCart(id);

            return Results.Ok(cart);
        });

        // Endpoint to test the grpc product client
        order.MapGet("/grpc-prd-test/{id}", (string id, IGrpcClient cartClient) =>
        {
            var product = cartClient.GetProduct(id);

            return Results.Ok(product);
        })
            .RequireAuthorization();
    }
}
