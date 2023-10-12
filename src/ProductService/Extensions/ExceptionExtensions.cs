using Microsoft.AspNetCore.Diagnostics;
using Shared.Exceptions;
using System.Text.Json;

namespace ProductService.Extensions;

public static class ExceptionExtensions
{
    public static void ConfigureExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(builder =>
        {
            builder.Run(async context =>
            {
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if(contextFeature is not null)
                {
                    context.Response.StatusCode = contextFeature.Error switch
                    {
                        ApiNotFoundException => StatusCodes.Status404NotFound,
                        ApiBadRequestException => StatusCodes.Status400BadRequest,
                        ApiFluentValidationException => StatusCodes.Status422UnprocessableEntity,
                        _ => StatusCodes.Status500InternalServerError
                    };

                    if(contextFeature.Error is ApiFluentValidationException exception)
                    {
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new { exception.Errors }));
                    }
                    else
                    {
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            message = contextFeature.Error.Message,
                            Details = contextFeature.Error.StackTrace
                        }));
                    }
                }
            });
        });
    }
}
