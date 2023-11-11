using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Entities.OrderAggregate;
using OrderService.Requests;
using OrderService.Response;
using OrderService.Services.Contracts;
using Shared.Exceptions;
using System.Security.Claims;

namespace OrderService.Services.Implementation;

public class OrderService : IOrderService
{
    private readonly OrderDbContext _context;
    private readonly IGrpcClient _grpcClient;
    private readonly IHttpContextAccessor _contextAccessor;

    public OrderService(OrderDbContext context, IGrpcClient grpcClient, 
        IHttpContextAccessor contextAccessor)
    {
        _context = context;
        _grpcClient = grpcClient;
        _contextAccessor = contextAccessor;
    }

    public async Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest orderRequest)
    {
        var cart = _grpcClient.GetShoppingCart(orderRequest.CartId) ??
            throw new ApiNotFoundException($"Shopping cart with id: {orderRequest.CartId} not found");

        var orderItems = new List<OrderItem>();

        var orderItemsResponse = new List<OrderItemsResponse>();

        foreach(var item in cart.Items)
        {
            var productItem = await _context.Product.FirstOrDefaultAsync(x => x.Id == Guid.Parse(item.ProductId)) ?? 
                _grpcClient.GetProduct(item.ProductId);

            if (productItem is null)
                throw new ApiNotFoundException($"Product with id: {productItem.Id} not found");

            var productItemOrdered = new ProductItemOrdered
            {
                ImageUrl = productItem.ImageUrl,
                ProductName = productItem.Name,
                ProductId = productItem.Id
            };

            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                ItemOrdered = productItemOrdered,
                Price = productItem.Price,
                Quantity = item.Quantity
            };

            var orderItemResponse = new OrderItemsResponse
            {
                Brand = productItem.Brand,
                Category = productItem.Category,
                ImageUrl = productItem.ImageUrl,
                OrderItemId = orderItem.Id.ToString(),
                ProductName = productItem.Name,
                Price = productItem.Price,
                Quantity = item.Quantity
            };

            orderItems.Add(orderItem);

            orderItemsResponse.Add(orderItemResponse);
        }

        var deliveryMethod = await _context.DeliveryMethod
            .FirstOrDefaultAsync(x => x.Id == Guid.Parse(orderRequest.DeliveryMethodId)) ??
            throw new ApiNotFoundException($"Delivery method with id: {orderRequest.DeliveryMethodId} not found");

        var deliveryAddress = new Address
        {
            City = orderRequest.DeliveryAddress.City,
            State = orderRequest.DeliveryAddress.State,
            ZipCode = orderRequest.DeliveryAddress.ZipCode,
            Street = orderRequest.DeliveryAddress.Street,
        };

        var order = new Order
        {
            OrderItems = orderItems,
            DeliveryMethod = deliveryMethod,
            DeliveryAddress = deliveryAddress,
            Id = Guid.NewGuid(),
            BuyerEmail = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email),
            OrderDate = DateTime.UtcNow,
            OrderStatus = Enums.OrderStatus.Pending,
            PaymentStatus = Enums.PaymentStatus.Pending,
            SubTotal = orderItems.Sum(x => x.Quantity * x.Price)
        };

        await _context.Order.AddAsync(order);

        var result = await _context.SaveChangesAsync();

        if (result > 0)
            _grpcClient.DeleteCart(orderRequest.CartId);
        else
            throw new ApiBadRequestException("An error occurred while creating this order");

        return new CreateOrderResponse
        {
            PaymentStatus = Enums.PaymentStatus.Pending.ToString(),
            OrderStatus = Enums.OrderStatus.Pending.ToString(),
            DeliveryAddress = new AddressResponse
            {
                City = order.DeliveryAddress.City,
                State = order.DeliveryAddress.State,
                Street = order.DeliveryAddress.Street,
                ZipCode = order.DeliveryAddress.ZipCode
            },
            Subtotal = order.SubTotal,
            Total = order.GetTotal(),
            OrderDate = order.OrderDate,
            OrderId = order.Id.ToString(),
            OrderItems = orderItemsResponse
        };
    }

    public async Task<GetOrderResponse> GetOrderByIdAsync(Guid id)
    {
        var order = await _context.Order
                    .Include(x => x.DeliveryMethod)
                    .Include(x => x.OrderItems)
                    .FirstOrDefaultAsync(x => x.Id == id) ??
                    throw new ApiNotFoundException($"Order with id: {id} not found");

        List<OrderItemsResponse> itemsResponse = new();

        foreach(var item in order.OrderItems)
        {
            OrderItemsResponse itemResponse = new()
            {
                Price = item.Price,
                Quantity = item.Quantity,
                ProductName = item.ItemOrdered.ProductName,
                ImageUrl = item.ItemOrdered.ImageUrl,
                OrderItemId = item.Id.ToString()
            };

            itemsResponse.Add(itemResponse);
        }

        return new GetOrderResponse
        {
            OrderItems = itemsResponse,
            DeliveryCharges = order.DeliveryMethod.Price,
            SubTotal = itemsResponse.Sum(x => x.Quantity * x.Price),
            Total = order.GetTotal(),
            OrderStatus = order.OrderStatus.ToString(),
            PaymentStatus = order.PaymentStatus.ToString(),
            OrderId = order.Id.ToString()
        };
    }
       
    public async Task<IReadOnlyList<GetOrderResponse>> GetOrdersForUserAsync()
    {
        var email = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

        var orders = await _context.Order
            .Include(x => x.DeliveryMethod)
            .Include(x => x.OrderItems)
            .Where(x => x.BuyerEmail == email)
            .ToListAsync();

        List<GetOrderResponse> ordersResponse = new();

        foreach(var order in orders)
        {
            GetOrderResponse orderResponse = new()
            {
                OrderId = order.Id.ToString(),
                SubTotal = order.OrderItems.Sum(x => x.Quantity * x.Price),
                Total = order.GetTotal(),
                DeliveryCharges = order.DeliveryMethod.Price,
                OrderStatus = order.OrderStatus.ToString(),
                PaymentStatus = order.PaymentStatus.ToString()
            };

            ordersResponse.Add(orderResponse);
        }

        return ordersResponse;
    }
        
}
