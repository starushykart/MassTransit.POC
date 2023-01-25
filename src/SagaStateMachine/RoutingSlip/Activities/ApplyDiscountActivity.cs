using MassTransit;

namespace SagaStateMachine.RoutingSlip.Activities;

public class ApplyDiscountActivity : IActivity<ApplyDiscountArgs, ApplyDiscountLog>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<ApplyDiscountArgs> context)
    {
        // do some stuff
        return context.Completed<ApplyDiscountLog>(new { context.Arguments.OrderId });
    }

    public async Task<CompensationResult> Compensate(CompensateContext<ApplyDiscountLog> context)
    {
        return context.Compensated();
    }
}

public record ApplyDiscountArgs(Guid OrderId);
public record ApplyDiscountLog(Guid OrderId);