namespace OrderService.Entities.OrderAggregate;

public sealed record Address(
    string ZipCode,
    string Street,
    string City,
    string State
    );
