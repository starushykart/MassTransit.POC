using MassTransit.Saga.StateMachineSaga.Activities;
using MassTransit.Saga.StateMachineSaga.Events;

namespace MassTransit.Saga.StateMachineSaga;

public class OrderSaga : MassTransitStateMachine<OrderState>
{
    public OrderSaga()
    {
        InstanceState(x => x.CurrentState);
        Event(() => SubmitOrder, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => OrderProcessing, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => OrderCancelled, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => OrderCompleted, x => x.CorrelateById(context => context.Message.OrderId));

        Schedule(() => OrderExpired, x => x.ExpirationToken, x =>
        {
            x.Delay = TimeSpan.FromSeconds(50);
            x.Received = e => e.CorrelateById(context => context.Message.OrderId);
        });
        
        Initially(
            When(SubmitOrder)
                .Activity(x => x.OfType<SaveOrderActivity>())
                .TransitionTo(Submitted)
                .Schedule(OrderExpired, context => new OrderExpired { OrderId = context.Saga.CorrelationId })
        );

        During(Submitted, 
            When(OrderProcessing)
                .TransitionTo(Processing));
        
        During(Processing, When(OrderCompleted)
            .TransitionTo(Completed));

        DuringAny(When(OrderCancelled)
            .TransitionTo(Cancelled)
            .Then(x => x.Saga.CancelledAt = DateTime.Now)
            .Unschedule(OrderExpired));
        
        DuringAny(
            When(OrderExpired!.Received)
                .TransitionTo(Expired));
        
        CompositeEvent(() => OrderReady, x => x.ReadyEventStatus, SubmitOrder, OrderProcessing, OrderCompleted);

        DuringAny(
            When(OrderReady)
                .Finalize());
    }
    
    // saga states
    public State Submitted { get; private set; }
    public State Processing { get; private set; }
    public State Cancelled { get; private set; }
    public State Completed { get; private set; }
    public State Expired { get; private set; }

    // saga events
    public Event<SubmitOrder> SubmitOrder { get; private set; }
    public Event<OrderProcessing> OrderProcessing { get; private set; }
    public Event<OrderCancelled> OrderCancelled { get; private set; }
    public Event<OrderCompleted> OrderCompleted { get; private set; }
    public Event OrderReady { get; private set; }

    public Schedule<OrderState, OrderExpired> OrderExpired { get; private set; }
}