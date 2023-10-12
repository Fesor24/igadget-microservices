using MediatR;
using ProductService.Definitions.Contracts;
using ProductService.Features.Product.Commands.Create;
using ProductService.Features.Product.Commands.Update;
using ProductService.Features.Product.Queries.GetProductById;
using ProductService.Features.Product.Queries.GetProducts;
using ProductService.Request;

namespace ProductService.Definitions;

public class ProductEndpointDefinition : IEndpointDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        var product = app.MapGroup("/api/products");

        product.MapGet("/", GetProductsAsync)
            .WithTags("Products");

        product.MapGet("/{id}", GetProductByIdAsync)
            .WithTags("Products")
            .WithName("GetProductById");

        product.MapPost("/", CreateProductAsync)
            .WithTags("Products");

        product.MapPut("/{id}", UpdateProductAsync)
            .WithTags("Products");

    }

    private async Task<IResult> GetProductsAsync(IMediator mediator)
    {
        var request = new GetProductsRequest();

        var result = await mediator.Send(request);

        return Results.Ok(result);
    }

    private async Task<IResult> GetProductByIdAsync(IMediator mediator,  Guid id)
    {
        var request = new GetProductByIdRequest { Id = id };

        var result = await mediator.Send(request);

        return Results.Ok(result);
    }

    private async Task<IResult> CreateProductAsync(IMediator mediator, CreateProductRequest product)
    {
        var request = new CreateProductCommand
        {
            Name = product.Name,
            Description = product.Description,
            ImageUrl = product.ImageUrl,
            BrandId = product.BrandId,
            Price = product.Price,
            CategoryId = product.CategoryId,
            YearOfRelease = product.YearOfRelease
        };

        var id = await mediator.Send(request);

        return Results.CreatedAtRoute("GetProductById", new {id}, request);

    }

    private async Task<IResult> UpdateProductAsync(IMediator mediator, Guid id, UpdateProductRequest product)
    {
        var request = new UpdateProductCommand
        {
            Id = id,
            Name = product.Name,
            Description = product.Description,
            ImageUrl = product.ImageUrl
        };

        await mediator.Send(request);

        return Results.NoContent();
    }
}
