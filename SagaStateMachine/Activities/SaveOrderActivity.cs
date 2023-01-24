using MassTransit;
using Microsoft.Extensions.Logging;
using SagaStateMachine.Events;

namespace SagaStateMachine.Activities;

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
        // throw new Exception();
        _logger.LogInformation("Order {OrderId} saved to database", context.Saga.CorrelationId);
        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<OrderState, SubmitOrder, TException> context,
        IBehavior<OrderState, SubmitOrder> next) where TException : Exception
    {
        _logger.LogInformation("Order {OrderId} compensation called", context.Saga.CorrelationId);
        return next.Faulted(context);
    }
}