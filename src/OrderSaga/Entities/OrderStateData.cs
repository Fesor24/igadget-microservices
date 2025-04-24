using System;
using MassTransit;

namespace OrderSaga.Entities;

public class OrderStateData : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public Guid OrderId { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public bool CustomerNotified { get; set; }
    // we can add other props specific to business needs...
}