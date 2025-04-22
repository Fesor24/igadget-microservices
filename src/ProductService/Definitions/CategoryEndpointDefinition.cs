using MediatR;
using ProductService.Definitions.Contracts;
using ProductService.Features.Categories.Command.Create;
using ProductService.Features.Categories.Queries.GetCategories;
using ProductService.Features.Categories.Queries.GetCategoryById;
using ProductService.Request;

namespace ProductService.Definitions;

public class CategoryEndpointDefinition : IEndpointDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        var category = app.MapGroup("/api/categories");

        const string TAGNAME = "Categories";

        category.MapGet("/", GetCategoriesAsync)
            .WithTags(TAGNAME);

        category.MapGet("/{id}", GetCategoryByIdAsync)
            .WithTags(TAGNAME)
            .WithName("GetCategoryById");

        category.MapPost("/", CreateCategoryAsync)
            .WithTags(TAGNAME);
    }

    private async Task<IResult> GetCategoriesAsync(IMediator mediator)
    {
        var request = new GetCategoriesRequest();

        var result = await mediator.Send(request);

        return Results.Ok(result);
    }

    private async Task<IResult> GetCategoryByIdAsync(IMediator mediator, Guid id)
    {
        var request = new GetCategoryByIdRequest(id);

        var result = await mediator.Send(request);

        return TypedResults.Ok(result);
    }

    private async Task<IResult> CreateCategoryAsync(IMediator mediator, CreateCategoryRequest category)
    {
        var request = new CreateCategoryCommand(category.Name);

        var id = await mediator.Send(request);

        return Results.CreatedAtRoute("GetCategoryById", new {id}, new {id, request.Name});
    }
}
