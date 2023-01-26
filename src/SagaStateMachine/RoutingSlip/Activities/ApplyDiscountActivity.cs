using MassTransit;
using Microsoft.Extensions.Logging;

namespace SagaStateMachine.RoutingSlip.Activities;

public class ApplyDiscountActivity : IActivity<ApplyDiscountArgs, ApplyDiscountLog>
{
    private readonly ILogger<ApplyDiscountActivity> _logger;

    public ApplyDiscountActivity(ILogger<ApplyDiscountActivity> logger)
    {
        _logger = logger;
    }
    
    public async Task<ExecutionResult> Execute(ExecuteContext<ApplyDiscountArgs> context)
    {
        _logger.LogInformation("Routing slip. {Activity} for order {OrderId} executed", nameof(ApplyDiscountActivity), context.Arguments.OrderId);
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