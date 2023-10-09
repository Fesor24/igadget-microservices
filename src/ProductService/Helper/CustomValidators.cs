namespace ProductService.Helper;

public class CustomValidators
{
    public static bool IsValidGuid(string value) =>
        Guid.TryParse(value, out _);
}
