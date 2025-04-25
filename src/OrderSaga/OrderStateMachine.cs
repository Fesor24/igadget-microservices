using System;
using MassTransit;
using OrderSaga.Entities;
using Shared.Contracts;
using Shared.Contracts.Commands;

namespace OrderSaga;

public sealed class OrderStateMachine : MassTransitStateMachine<OrderStateData>
{
    public State Pending { get; private set; }
    public State Paid { get; private set; }
    public State CancellationRequested { get; private set; }
    public State Cancelled { get; private set; }
    public State Completed { get; private set; }    
    
    public Event<OrderCreated> OrderCreated { get; private set; }
    public Event<OrderPaid> OrderPaid { get; private set; }
    public Event<CancellationRequested> OrderCancellationRequested { get; private set; }
    public Event<OrderCancelled> OrderCancelled { get; private set; }
    public Event<OrderCompleted> OrderCompleted { get; private set; }

    public OrderStateMachine()
    {
        // specify the current state...
        InstanceState(x => x.CurrentState);
        
        // How our events are correlated...
        Event(() => OrderCreated,
            x => x.CorrelateById(context => context.Message.OrderId));
        
        Event(() => OrderPaid,
            x => x.CorrelateById(context => context.Message.OrderId));

        Event(() => OrderCancellationRequested,
            x => x.CorrelateById(context => context.Message.OrderId));
        
        Event(() => OrderCancelled,
            x => x.CorrelateById(context => context.Message.OrderId));

        Event(() => OrderCompleted,
            x => x.CorrelateById(context => context.Message.OrderId));
        
        Initially(
            When(OrderCreated) // when an order is created...an OrderCreated message was published...
                .Then(context =>
                {
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.OrderStatus = OrderStatus.Pending;
                    context.Saga.CustomerNotified = false;
                    
                    Console.WriteLine($"Order Created at {DateTime.Now:MM/dd/yyyy} with Id: {context.Message.OrderId}");
                })
                .TransitionTo(Pending)); // then we transition it to pending state...
        
        // when an order is pending, the next course of actions could be:
        // either it has now been paid for or an order cancellation has been requested
        // we handle both instances...
        During(Pending, 
            When(OrderPaid)
                .Then(context =>
                {
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.OrderStatus = OrderStatus.Paid;
                    // update other relevant props
                    
                    Console.WriteLine($"Order Paid at {DateTime.Now:MM/dd/yyyy} with Id: {context.Message.OrderId}");
                })
                .TransitionTo(Paid)
                .Publish(context => 
                    new OrderInvoiceNeeded(context.Message.OrderId)// if we want to publish an event...in this case send an invoice when a customer pays
                ).Publish(context => new OrderShipped(context.Message.OrderId)),
            
            // we handle the second part of the scenario...
            When(OrderCancellationRequested)
                .Then(context =>
                {
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.OrderStatus = OrderStatus.CancellationRequested;
                    
                    Console.WriteLine($"Order cancellation requested at {DateTime.Now:g} with Id: {context.Message.OrderId}");
                })
                .TransitionTo(CancellationRequested)
                .Publish(context => new OrderCancelled(context.Message.OrderId)) // say we want to notify the customer that the order has been cancelled...
            );
        
        // when it is in a paid state and an order cancellation is requested
        // we refund user...
        During(Paid,
            When(OrderCancellationRequested)
                .Then(context =>
                {
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.OrderStatus = OrderStatus.CancellationRequested;
                })
                .Publish(context => new OrderPaymentRefund(context.Message.OrderId))
                .Publish(context => new OrderCancelled(context.Message.OrderId))
            
        );
        
        During(CancellationRequested,
            When(OrderCancelled)
                .Then(context =>
                {
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.OrderStatus = OrderStatus.Cancelled;
                    
                    Console.WriteLine($"Order cancelled at {DateTime.Now:g} with Id: {context.Message.OrderId}");
                }).TransitionTo(Cancelled)
            );
        
        During(CancellationRequested, Cancelled,
            Ignore(OrderCancellationRequested));
        
        During(Paid,
            Ignore(OrderPaid));
        
        // During any state
        // when an OrderCompleted event is published...
        DuringAny(When(OrderCompleted).Finalize()); // transitions to the final state...
        SetCompletedWhenFinalized();// removes the state instance from db...
    }
}