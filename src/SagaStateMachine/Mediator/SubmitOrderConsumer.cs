using MassTransit;
using SagaStateMachine.Mediator.Models;
using SagaStateMachine.StateMachine.Events;

namespace SagaStateMachine.Mediator;

public class SubmitOrderConsumer : IConsumer<ISubmitOrder>
{
    private readonly IBus _bus;

    public SubmitOrderConsumer(IBus bus)
    {
        _bus = bus;
    }
    
    public async Task Consume(ConsumeContext<ISubmitOrder> context)
    {
        await _bus.Publish<OrderSubmitted>(new { context.Message.OrderId });
        
        await context.RespondAsync<ISubmitOrderResponse>(new
        {
            Text = $"Order submitted: {context.Message.OrderId} - {DateTime.UtcNow}"
        });
    }
}