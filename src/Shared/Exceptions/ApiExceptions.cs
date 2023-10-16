namespace Shared.Exceptions;
public partial class ApiNotFoundException : Exception
{
    public ApiNotFoundException(string message) : base(message){ }
}

public partial class ApiBadRequestException : Exception
{
    public ApiBadRequestException(string message) : base(message) { }
}

public partial class ApiNotAuthorizedException : Exception
{
    public ApiNotAuthorizedException(string message) : base(message) { }
}

public partial class ApiFluentValidationException : Exception
{
    public ApiFluentValidationException(IReadOnlyDictionary<string, string[]> errors): 
        base("One or moe validation errors occurred")
    {
        Errors = errors;
    }

    public IReadOnlyDictionary<string, string[]> Errors { get; }
}
