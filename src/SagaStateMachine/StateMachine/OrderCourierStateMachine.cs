using MassTransit;
using Microsoft.Extensions.Logging;
using SagaStateMachine.StateMachine.Activities;
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
        Event(() => ApproveOrder, x => x.CorrelateById(context => context.Message.OrderId));

        Schedule(() => OrderExpired, x => x.ExpirationToken, x =>
        {
            x.Delay = TimeSpan.FromMinutes(10);
            x.Received = e => e.CorrelateById(context => context.Message.OrderId);
        });

        Initially(
            When(OrderSubmitted)
                .Then(x => x.Saga.SubmittedAt = DateTime.Now)
                .Then(x => logger.LogInformation("State machine. Order {OrderId} submitted", x.Message.OrderId))
                .TransitionTo(Submitted));
        
        During(Submitted, 
            When(ProcessOrder)
                //.Activity(x=>x.OfType<ProcessOrderActivity>())
                .Then(x=> _logger.LogInformation("State machine. {Activity} with routing slip for Order {OrderId} executing", nameof(ProcessOrderActivity), x.Saga.CorrelationId))
                .TransitionTo(Processed));
        
        During(Processed, 
            When(ApproveOrder)
                .Then(x=> _logger.LogInformation("State machine. Order {OrderId} approved", x.Saga.CorrelationId)));
        
        CompositeEvent(() => OrderReady, x => x.ReadyEventStatus, ProcessOrder, ApproveOrder);

        DuringAny(
            When(OrderExpired!.Received)
                .Then(x => x.Saga.ExpiredAt = DateTime.Now)
                .TransitionTo(Expired)
                .Finalize(),
            When(OrderReady)
                .TransitionTo(Ready)
                .Then(x=> _logger.LogInformation("State machine. Order {OrderId} ready", x.Saga.CorrelationId))
                .Finalize(),
            When(OrderCancelled)
                .TransitionTo(Cancelled)
                .Finalize());
        
        WhenEnter(Submitted, x => x.Schedule(OrderExpired, context => new OrderExpired { OrderId = context.Saga.CorrelationId }));
        WhenEnter(Ready, x => x.Unschedule(OrderExpired));
        WhenEnter(Cancelled, x => x.Unschedule(OrderExpired));
    }

    public State Submitted { get; private set; }
    public State Processed { get; private set; }
    public State Ready { get; private set; }
    public State Expired { get; private set; }
    public State Cancelled { get; private set; }

    public Event<OrderSubmitted> OrderSubmitted { get; private set; }
    public Event<ProcessOrder> ProcessOrder { get; private set; }
    public Event<ApproveOrder> ApproveOrder { get; private set; }
    public Event<OrderCancelled> OrderCancelled { get; private set; }
    public Schedule<OrderState, OrderExpired> OrderExpired { get; private set; }
    public Event OrderReady { get; private set; }
}