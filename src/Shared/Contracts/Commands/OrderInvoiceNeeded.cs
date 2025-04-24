namespace Shared.Contracts.Commands;

public sealed record OrderInvoiceNeeded(
    Guid OrderId);