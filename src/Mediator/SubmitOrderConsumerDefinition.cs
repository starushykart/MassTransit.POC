using MassTransit;

namespace Mediator;

public class SubmitOrderConsumerDefinition : ConsumerDefinition<SubmitOrderConsumer>
{
    public SubmitOrderConsumerDefinition()
    {
        ConcurrentMessageLimit = 10;
    }
    
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<SubmitOrderConsumer> consumerConfigurator)
    {
        endpointConfigurator
            .UseMessageRetry(x => x.SetRetryPolicy(y => y.Immediate(2)));
    }
}