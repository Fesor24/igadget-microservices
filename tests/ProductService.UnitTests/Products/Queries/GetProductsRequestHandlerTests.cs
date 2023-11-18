using AutoFixture;
using AutoMapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProductService.DataAccess.Contracts;
using ProductService.Features.Product.Queries.GetProductById;
using ProductService.Features.Product.Queries.GetProducts;
using ProductService.Helper;
using ProductService.Models;
using ProductService.Response;

namespace ProductService.UnitTests.Products.Queries;
public class GetProductsRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;

    public GetProductsRequestHandlerTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();

        var mockMapper = new MapperConfiguration(mc =>
        {
            mc.AddMaps(typeof(MappingProfiles).Assembly);
        }).CreateMapper().ConfigurationProvider;

        _mapper = new Mapper(mockMapper);

        _fixture = new Fixture();
    }

    [Fact]
    public async Task Handle_Should_ReturnProductList()
    {
        var products = _fixture.CreateMany<ProductModel>(10).ToList();

        _unitOfWork.Setup(unit => unit.ProductRepository.GetProductsDetails())
            .ReturnsAsync((IReadOnlyList<ProductModel>)products);

        var request = new GetProductsRequest();

        var handler = new GetProductsRequestHandler(_unitOfWork.Object, _mapper);

        var result = await handler.Handle(request, default);

        using var _ = new AssertionScope();

        result.Should().BeOfType<List<GetProductResponse>>();

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_Should_ReturnProduct()
    {
        var product = _fixture.Create<ProductModel>();

        _unitOfWork.Setup(unit => unit.ProductRepository.GetProductDetails(It.IsAny<Guid>()))
            .ReturnsAsync(product);

        var request = new GetProductByIdRequest { Id = It.IsAny<Guid>()};

        var handler = new GetProductByIdRequestHandler(_unitOfWork.Object, _mapper);

        var result = await handler.Handle(request, default);

        using var _ = new AssertionScope();

        result.Should().BeOfType<GetProductResponse>()
            .Should().NotBeNull();
    }
}
