using MediatR;
using ProductService.Features.Products.Commands.Create;
using ProductService.Features.Products.Commands.Delete;
using ProductService.Features.Products.Commands.Update;
using ProductService.Features.Products.Queries.GetProductById;
using ProductService.Features.Products.Queries.GetProducts;
using ProductService.Request;
using Shared.Exceptions;

namespace ProductService.Definitions;

public class ProductEndpointDefinition : ProductService.Definitions.Contracts.IEndpointDefinition
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

        product.MapDelete("/{id}", DeleteProductAsync)
            .WithTags("Products");

    }

    internal async Task<IResult> GetProductsAsync(IMediator mediator)
    {
        var request = new GetProductsRequest();

        var result = await mediator.Send(request);

        return Results.Ok(result);
    }

    internal async Task<IResult> GetProductByIdAsync(IMediator mediator, string id)
    {
        Guid productId;

        var validGuid = Guid.TryParse(id, out productId);

        if (!validGuid) throw new ApiBadRequestException($"Invalid param: {id}");

        var request = new GetProductByIdRequest(productId);

        var result = await mediator.Send(request);

        return Results.Ok(result);
    }

    internal async Task<IResult> CreateProductAsync(IMediator mediator, CreateProductRequest productRequest)
    {
        var request = new CreateProductCommand
        {
            Name = productRequest.Name,
            Description = productRequest.Description,
            ImageUrl = productRequest.ImageUrl,
            BrandId = productRequest.BrandId,
            Price = productRequest.Price,
            CategoryId = productRequest.CategoryId,
            YearOfRelease = productRequest.YearOfRelease
        };

        var product = await mediator.Send(request);

        return Results.CreatedAtRoute("GetProductById", new {product.Id}, product);

    }

    internal async Task<IResult> UpdateProductAsync(IMediator mediator, string id, UpdateProductRequest product)
    {
        Guid productId;

        var validGuid = Guid.TryParse(id, out productId);

        if (!validGuid) throw new ApiBadRequestException($"Invalid product id: {id}");

        var request = new UpdateProductCommand
        {
            Id = productId,
            Name = product.Name,
            Description = product.Description,
            ImageUrl = product.ImageUrl,
            Price = product.Price
        };

        await mediator.Send(request);

        return Results.NoContent();
    }

    internal async Task<IResult> DeleteProductAsync(IMediator mediator, Guid id)
    {
        await mediator.Send(new DeleteProductCommand(id));

        return Results.NoContent();
    }
}
