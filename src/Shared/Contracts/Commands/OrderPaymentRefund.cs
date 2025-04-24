namespace Shared.Contracts.Commands;

public sealed record OrderPaymentRefund(
    Guid OrderId
    );