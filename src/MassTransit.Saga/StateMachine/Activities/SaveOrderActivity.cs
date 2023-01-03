using MassTransit.Saga.StateMachine.Events;

namespace MassTransit.Saga.StateMachine.Activities;

public class SaveOrderActivity : IStateMachineActivity<OrderState, SubmitOrder>
{
    private readonly ILogger<SaveOrderActivity> _logger;

    public SaveOrderActivity(ILogger<SaveOrderActivity> logger)
    {
        _logger = logger;
    }
    
    public void Probe(ProbeContext context)
    {
        context.CreateScope("publish-order-submitted");
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<OrderState, SubmitOrder> context, IBehavior<OrderState, SubmitOrder> next)
    {
        // do database saving
        _logger.LogInformation("Order {OrderId} saved to database", context.Saga.CorrelationId);
        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<OrderState, SubmitOrder, TException> context,
        IBehavior<OrderState, SubmitOrder> next) where TException : Exception
    {
        return next.Faulted(context);
    }
}