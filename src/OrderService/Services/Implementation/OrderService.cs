using Microsoft.EntityFrameworkCore;
using OrderService.DataAccess.Contracts;
using OrderService.DataAccess.Specifications.Delivery;
using OrderService.DataAccess.Specifications.Order;
using OrderService.DataAccess.Specifications.Products;
using OrderService.Entities;
using OrderService.Entities.OrderAggregate;
using OrderService.Requests;
using OrderService.Response;
using OrderService.Services.Contracts;
using Shared.Exceptions;
using System.Security.Claims;
using OrderEntity = OrderService.Entities.OrderAggregate.Order;

namespace OrderService.Services.Implementation;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGrpcClient _grpcClient;
    private readonly IHttpContextAccessor _contextAccessor;

    public OrderService(IUnitOfWork unitOfWork, IGrpcClient grpcClient, 
        IHttpContextAccessor contextAccessor)
    {
        _unitOfWork = unitOfWork;
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
            var productSpec = new GetProductSpecification(Guid.Parse(item.ProductId));

            var productItem = await _unitOfWork.Repository<Product>().GetAsync(productSpec) ??
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

        var deliverySpec = new GetDeliverySpecification(Guid.Parse(orderRequest.DeliveryMethodId));

        var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(deliverySpec) ??
            throw new ApiNotFoundException($"Delivery method with id: {orderRequest.DeliveryMethodId} not found");

        var deliveryAddress = new Address
        {
            City = orderRequest.DeliveryAddress.City,
            State = orderRequest.DeliveryAddress.State,
            ZipCode = orderRequest.DeliveryAddress.ZipCode,
            Street = orderRequest.DeliveryAddress.Street,
        };

        OrderEntity order = new()
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

        await _unitOfWork.Repository<OrderEntity>().AddAsync(order);

        var result = await _unitOfWork.Complete();

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
        var spec = new GetOrderSpecification(id);

        var order = await _unitOfWork.Repository<OrderEntity>().GetAsync(spec) ??
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
            OrderId = order.Id.ToString(),
            Date = order.OrderDate.ToString("dd ddd MMM yyyy hh mm")
        };
    }
       
    public async Task<IReadOnlyList<GetOrderResponse>> GetOrdersForUserAsync()
    {
        var email = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

        var spec = new GetOrderSpecification(email);

        var orders = await _unitOfWork.Repository<OrderEntity>().GetAllAsync(spec);

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
                PaymentStatus = order.PaymentStatus.ToString(),
                Date = order.OrderDate.ToString("dd ddd MMM yyyy hh mm")
            };

            ordersResponse.Add(orderResponse);
        }

        return ordersResponse;
    }
        
}
