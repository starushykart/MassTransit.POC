using MassTransit;
using Microsoft.Extensions.Logging;
using SagaStateMachine.StateMachine.Events;
using Exception = System.Exception;

namespace SagaStateMachine.RoutingSlip.Activities;

public class ProcessPaymentActivity : IActivity<ProcessPaymentArgs, ProcessPaymentLog>
{
    private readonly ILogger<ProcessPaymentActivity> _logger;
    private readonly IBus _bus;

    public ProcessPaymentActivity(IBus bus, ILogger<ProcessPaymentActivity> logger)
    {
        _logger = logger;
        _bus = bus;
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<ProcessPaymentArgs> context)
    {
        await _bus.Publish(new ApproveOrder(context.Arguments.OrderId)); 
        _logger.LogInformation("Routing slip. {Activity} for order {OrderId} executed",
                nameof(ProcessPaymentActivity), context.Arguments.OrderId);
        
        return context.Completed<ProcessPaymentLog>(new { context.Arguments.OrderId });
    }

    public async Task<CompensationResult> Compensate(CompensateContext<ProcessPaymentLog> context)
    {
        return context.Compensated();
    }
}

public record ProcessPaymentArgs(Guid OrderId);
public record ProcessPaymentLog(Guid OrderId);