using MassTransit;
using Microsoft.Extensions.Logging;
using SagaStateMachine.StateMachine.Events;

namespace SagaStateMachine.StateMachine;

public class OrderCourierStateMachine : MassTransitStateMachine<OrderState>
{
    private readonly ILogger<OrderCourierStateMachine> _logger;

    public OrderCourierStateMachine(ILogger<OrderCourierStateMachine> logger)
    {
        _logger = logger;
        
        InstanceState(x => x.CurrentState);
        Event(() => OrderSubmitted, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => OrderCancelled, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => ProcessOrder, x => x.CorrelateById(context => context.Message.OrderId));

        Schedule(() => OrderExpired, x => x.ExpirationToken, x =>
        {
            x.Delay = TimeSpan.FromSeconds(50);
            x.Received = e => e.CorrelateById(context => context.Message.OrderId);
        });

        Initially(
            When(OrderSubmitted)
                //.Schedule(OrderExpired, context => new OrderExpired { OrderId = context.Saga.CorrelationId })
                .Then(x => x.Saga.SubmittedAt = DateTime.Now)
                .Then(x => logger.LogInformation("Order {OrderId} submitted", x.Message.OrderId))
                .TransitionTo(Submitted));
        
        During(Submitted, 
            When(ProcessOrder)
                //.Activity(x=>x.OfType<ProcessOrderActivity>())
                .TransitionTo(Processed));
        
        DuringAny(
            When(OrderExpired!.Received)
                .Then(x => x.Saga.ExpiredAt = DateTime.Now)
                .TransitionTo(Expired)
                .Finalize(),
            When(OrderCancelled)
                .TransitionTo(Cancelled)
                .Finalize());

        //CompositeEvent(() => OrderReady, x => x.ReadyEventStatus, OrderSubmitted, ProcessOrder);

        WhenEnter(Submitted, x => x.Schedule(OrderExpired, context => new OrderExpired { OrderId = context.Saga.CorrelationId }));
        WhenEnter(Cancelled, x => x.Unschedule(OrderExpired));
    }

    public State Submitted { get; private set; }
    public State Processed { get; private set; }
    public State Expired { get; private set; }
    public State Cancelled { get; private set; }

    public Event<OrderSubmitted> OrderSubmitted { get; private set; }
    public Event<ProcessOrder> ProcessOrder { get; private set; }
    public Event<OrderCancelled> OrderCancelled { get; private set; }
    public Schedule<OrderState, OrderExpired> OrderExpired { get; private set; }
    //public Event OrderReady { get; private set; }
}