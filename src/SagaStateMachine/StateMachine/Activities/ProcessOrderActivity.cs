using MassTransit;
using SagaStateMachine.StateMachine.Events;

namespace SagaStateMachine.StateMachine.Activities;

public class ProcessOrderActivity : IStateMachineActivity<OrderState, ProcessOrder>
{
    public async Task Execute(BehaviorContext<OrderState, ProcessOrder> context, IBehavior<OrderState, ProcessOrder> next)
    {
        var builder = new RoutingSlipBuilder(NewId.NextGuid());

        builder.AddActivity("ApplyDiscountActivity", new Uri("amazonsqs://localhost:4566/000000000000/ApplyDiscount_execute"), new
        {
            context.Message.OrderId
        });

        builder.AddActivity("ProcessPayment", new Uri("amazonsqs://localhost:4566/000000000000/ProcessPayment_execute"), new
        {
            context.Message.OrderId
        });
        
        var routingSlip = builder.Build();
        
        await context.Execute(routingSlip);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<OrderState, ProcessOrder, TException> context, IBehavior<OrderState, ProcessOrder> next) where TException : Exception
    {
        return next.Faulted(context);
    }
    
    public void Probe(ProbeContext context) => context.CreateScope("publish-order-submitted");

    public void Accept(StateMachineVisitor visitor) => visitor.Visit(this);
}