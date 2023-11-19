using AutoFixture;
using Moq;
using OrderService.DataAccess.Contracts;
using OrderService.DataAccess.Specifications.Order;
using OrderSvc = OrderService.Services.Implementation.OrderService;
using OrderEntity = OrderService.Entities.OrderAggregate.Order;
using Microsoft.AspNetCore.Http;
using OrderService.Services.Contracts;
using FluentAssertions;
using OrderService.Response;
using System.Security.Claims;

namespace OrderService.UnitTests.Order;
public class OrderTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IHttpContextAccessor> _contextAccessor;
    private readonly Mock<IGrpcClient> _grpcClient;
    public OrderTests()
    {
        _fixture = new Fixture();
        _unitOfWork = new Mock<IUnitOfWork>();
        _contextAccessor = new Mock<IHttpContextAccessor>();
        _grpcClient = new Mock<IGrpcClient>();
    }

    [Fact]
    public async Task GetOrder_WithValidId_ReturnSingleOrder()
    {
        OrderEntity order = _fixture.Create<OrderEntity>();

        _unitOfWork.Setup(un => un.Repository<OrderEntity>().GetAsync(It.IsAny<GetOrderSpecification>()))
            .ReturnsAsync(order);

        var orderService = new OrderSvc(_unitOfWork.Object, _grpcClient.Object, _contextAccessor.Object);

        var orderResponse = await orderService.GetOrderByIdAsync(It.IsAny<Guid>());

        orderResponse.Should().NotBeNull();

        orderResponse.Should().BeAssignableTo<GetOrderResponse>();
    }

    [Fact]
    public async Task GetOrders_WithValidEmail_ShouldReturnListOfOrders()
    {
        var orders = _fixture.CreateMany<OrderEntity>(2);

        _contextAccessor.Setup(ca => ca.HttpContext.User.Claims)
            .Returns(new List<Claim>
            {
                new Claim(ClaimTypes.Email, "test@mail.com")
            });

        _unitOfWork.Setup(un => un.Repository<OrderEntity>().GetAllAsync(It.IsAny<GetOrderSpecification>()))
            .ReturnsAsync(orders.ToList());

        var orderService = new OrderSvc(_unitOfWork.Object, _grpcClient.Object, _contextAccessor.Object);

        var orderResponse = await orderService.GetOrdersForUserAsync();

        orderResponse.Should().NotBeNullOrEmpty();

        orderResponse.Should().HaveCount(2);

        orderResponse.Should().BeAssignableTo<List<GetOrderResponse>>();

    }
}
