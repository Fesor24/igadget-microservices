using OrderService.Requests;
using OrderService.Services.Contracts;

namespace OrderService.Endpoints;

public class OrderEndpointDefinition : IEndpointDefinition
{
    public void RegisterEndpoints(WebApplication app)
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

        order.MapPost("/", CreateOrder)
            .RequireAuthorization();

        order.MapGet("/{orderId}", GetOrder)
            .RequireAuthorization();

        order.MapGet("/user", GetOrdersForUser)
            .RequireAuthorization();

    }

    private static async Task<IResult> CreateOrder(CreateOrderRequest orderRequest, IOrderService orderService)
    {
        var response = await orderService.CreateOrderAsync(orderRequest);

        return Results.Ok(response);
    }

    private static async Task<IResult> GetOrder(Guid orderId, IOrderService orderService) =>
        Results.Ok(await orderService.GetOrderByIdAsync(orderId));

    private static async Task<IResult> GetOrdersForUser(IOrderService orderService) =>
        Results.Ok(await orderService.GetOrdersForUserAsync());
}
