using MediatR;
using ProductService.Features.Product.Commands.Create;
using ProductService.Features.Product.Commands.Delete;
using ProductService.Features.Product.Commands.Update;
using ProductService.Features.Product.Queries.GetProductById;
using ProductService.Features.Product.Queries.GetProducts;
using ProductService.Request;

namespace ProductService.Definitions;

public class ProductEndpointDefinition : ProductService.Definitions.Contracts.IEndpointDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        var product = app.MapGroup("/api/products");

        product.MapGet("/", GetProductsAsync)
            .WithTags("Products")
            .RequireAuthorization();

        product.MapGet("/{id}", GetProductByIdAsync)
            .WithTags("Products")
            .WithName("GetProductById")
            .RequireAuthorization();

        product.MapPost("/", CreateProductAsync)
            .WithTags("Products")
            .RequireAuthorization();

        product.MapPut("/{id}", UpdateProductAsync)
            .WithTags("Products")
            .RequireAuthorization();

        product.MapDelete("/{id}", DeleteProductAsync)
            .WithTags("Products")
            .RequireAuthorization();

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

    private async Task<IResult> CreateProductAsync(IMediator mediator, CreateProductRequest productRequest)
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

    private async Task<IResult> DeleteProductAsync(IMediator mediator, Guid id)
    {
        await mediator.Send(new DeleteProductCommand(id));

        return Results.NoContent();
    }
}
