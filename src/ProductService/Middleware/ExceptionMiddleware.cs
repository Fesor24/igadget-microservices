using Shared.Exceptions;
using System.Net;
using System.Text.Json;

namespace ProductService.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";

            Type exceptionType = ex.GetType();

            if (exceptionType == typeof(ApiNotFoundException))
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            else if (exceptionType == typeof(ApiBadRequestException))
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            else
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var errors = JsonSerializer.Serialize(new { message = ex.Message, details = ex.StackTrace }, jsonOptions);

           await context.Response.WriteAsync(errors);
        }
    }
}
