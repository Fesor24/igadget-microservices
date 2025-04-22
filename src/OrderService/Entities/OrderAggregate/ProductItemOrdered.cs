namespace OrderService.Entities.OrderAggregate;

public sealed record ProductItemOrdered(
    Guid ProductId,
    string Name,
    string ImageUrl
);
