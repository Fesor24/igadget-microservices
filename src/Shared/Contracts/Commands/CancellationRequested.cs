namespace Shared.Contracts.Commands;

public sealed record CancellationRequested(
    Guid OrderId
    );