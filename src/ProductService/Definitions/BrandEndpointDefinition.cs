﻿using MediatR;
using ProductService.Definitions.Contracts;
using ProductService.Features.Brands.Command.Create;
using ProductService.Features.Brands.Queries.GetBrandById;
using ProductService.Features.Brands.Queries.GetBrands;
using ProductService.Request;

namespace ProductService.Definitions;

public class BrandEndpointDefinition : IEndpointDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        const string TAGNAME = "Brands";

        var brand = app.MapGroup("/api/brands");

        brand.MapGet("/", GetBrandsAsync)
            .WithTags(TAGNAME);

        brand.MapPost("/", CreateBrandAsync)
            .WithTags(TAGNAME);

        brand.MapGet("/{id}", GetBrandAsync)
            .WithName("GetBrandById")
            .WithTags(TAGNAME);
    }

    private async Task<IResult> GetBrandsAsync(IMediator mediator)
    {
        var request = new GetBrandsRequest();

        var results = await mediator.Send(request);

        return Results.Ok(results);
    }

    private async Task<IResult> CreateBrandAsync(IMediator mediator, CreateBrandRequest brand)
    {
        var request = new CreateBrandCommand(brand.Name);

        var id = await mediator.Send(request);

        return Results.CreatedAtRoute("GetBrandById", new {id}, new {id, request.Name});
    }

    private async Task<IResult> GetBrandAsync(IMediator mediator, Guid id)
    {
        var request = new GetBrandByIdRequest(id);

        var result = await mediator.Send(request);

        return TypedResults.Ok(result);
    }
}
