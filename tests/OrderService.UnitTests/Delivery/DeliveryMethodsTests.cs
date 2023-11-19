using AutoFixture;
using AutoMapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using OrderService.DataAccess.Contracts;
using OrderService.DataAccess.Specifications.Delivery;
using OrderService.Entities.OrderAggregate;
using OrderService.Helpers;
using OrderService.Response;
using OrderService.Services.Contracts;
using OrderService.Services.Implementation;

namespace OrderService.UnitTests.Delivery;
public class DeliveryMethodsTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;
    public DeliveryMethodsTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _fixture = new Fixture();

        var mapConfig = new MapperConfiguration(mc =>
        {
            mc.AddMaps(typeof(MappingProfiles).Assembly);
        }).CreateMapper().ConfigurationProvider;

        _mapper = new Mapper(mapConfig);
    }

    [Fact]
    public async Task GetDeliveryMethods_Return2Items()
    {
        var deliveryMethods = _fixture.CreateMany<DeliveryMethod>(2);

        _unitOfWork.Setup(un => un.Repository<DeliveryMethod>().GetAllAsync(It.IsAny<GetDeliverySpecification>()))
            .ReturnsAsync(deliveryMethods.ToList());

        var deliveryService = new DeliveryMethodService(_unitOfWork.Object, _mapper);

        var response = await deliveryService.GetDeliveryMethods();

        using var _ = new AssertionScope();

        response.Should().NotBeNullOrEmpty();

        response.Should().HaveCount(2);

        response.Should().BeAssignableTo<IReadOnlyList<GetDeliveryMethodResponse>>();
    }

    [Fact]
    public async Task GetDeliveryMethod_WithValidId_ReturnSingleItem()
    {
        var deliveryMethod = _fixture.Create<DeliveryMethod>();

        _unitOfWork.Setup(un => un.Repository<DeliveryMethod>().GetAsync(It.IsAny<GetDeliverySpecification>()))
            .ReturnsAsync(deliveryMethod);

        var deliveryService = new DeliveryMethodService(_unitOfWork.Object, _mapper);

        var response = await deliveryService.GetDeliveryMethod(It.IsAny<Guid>());

        response.Should().NotBeNull();

        response.Should().BeAssignableTo<GetDeliveryMethodResponse>();

        response.Name.Should().Be(deliveryMethod.Name);
    }
}
