using MediatR;
using ProductService.Definitions.Contracts;
using ProductService.Features.Product.Commands.Create;
using ProductService.Features.Product.Queries.GetProductById;
using ProductService.Features.Product.Queries.GetProducts;
using ProductService.Request;

namespace ProductService.Definitions;

public class ProductEndpointDefinition : IEndpointDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        var product = app.MapGroup("/api/products");

        product.MapGet("", GetProducts)
            .WithTags("Products");

        product.MapGet("/{id}", GetProductById)
            .WithTags("Products")
            .WithName("GetProductById");

        product.MapPost("", CreateProduct)
            .WithTags("Products");

    }

    private async Task<IResult> GetProducts(IMediator mediator)
    {
        var request = new GetProductsRequest();

        var result = await mediator.Send(request);

        return Results.Ok(result);
    }

    private async Task<IResult> GetProductById(IMediator mediator,  Guid id)
    {
        var request = new GetProductByIdRequest { Id = id };

        var result = await mediator.Send(request);

        return Results.Ok(result);
    }

    private async Task<IResult> CreateProduct(IMediator mediator, CreateProductRequest product)
    {
        var request = new CreateProductCommand
        {
            Id = Guid.Parse(product.Id),
            Name = product.Name,
            Description = product.Description,
            ImageUrl = product.ImageUrl,
            BrandId = Guid.Parse(product.BrandId),
            Price = product.Price,
            CategoryId = Guid.Parse(product.CategoryId),
            YearOfRelease = product.YearOfRelease
        };

        var result = await mediator.Send(request);

        return Results.CreatedAtRoute("GetProductById", result, request);

    }
}
