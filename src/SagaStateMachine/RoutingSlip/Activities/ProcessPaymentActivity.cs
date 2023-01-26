using MassTransit;

namespace SagaStateMachine.RoutingSlip.Activities;

public class ProcessPaymentActivity : IActivity<ProcessPaymentArgs, ProcessPaymentLog>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<ProcessPaymentArgs> context)
    {
        // do some stuff
        // throw new Exception("");
        return context.Completed<ProcessPaymentLog>(new { context.Arguments.OrderId });
    }

    public async Task<CompensationResult> Compensate(CompensateContext<ProcessPaymentLog> context)
    {
        return context.Compensated();
    }
}

public record ProcessPaymentArgs(Guid OrderId);
public record ProcessPaymentLog(Guid OrderId);