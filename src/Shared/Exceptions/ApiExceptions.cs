namespace Shared.Exceptions;
public partial class ApiNotFoundException : Exception
{
    public ApiNotFoundException(string message) : base(message){ }
}

public partial class ApiBadRequestException : Exception
{
    public ApiBadRequestException(string message) : base(message) { }
}
