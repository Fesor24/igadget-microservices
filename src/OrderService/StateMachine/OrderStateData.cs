using MassTransit;
using OrderService.Enums;

namespace OrderService.StateMachine;

public sealed class OrderStateData : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public Guid OrderId { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public bool CustomerNotified { get; set; }
    //public byte[] RowVersion { get; set; }
}