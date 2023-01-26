using MassTransit;
using Mediator.Models;
using Microsoft.Extensions.Logging;
using SagaStateMachine.StateMachine.Events;

namespace Mediator;

public class SubmitOrderConsumer : IConsumer<ISubmitOrder>
{
    private readonly IBus _bus;
    private readonly ILogger<SubmitOrderConsumer> _logger;

    public SubmitOrderConsumer(IBus bus, ILogger<SubmitOrderConsumer> logger)
    {
        _bus = bus;
        _logger = logger;
    }
    
    public async Task Consume(ConsumeContext<ISubmitOrder> context)
    {
        //throw new Exception();
        var id = NewId.Next();
        
        // some logic

        await _bus.Publish<OrderSubmitted>(new { OrderId = id });
        _logger.LogInformation("Order {OrderId} submitted", id);

        await context.RespondAsync<ISubmitOrderResponse>(new
        {
            OrderId = id
        });
    }
}