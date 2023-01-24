using MassTransit;
using Mediator.Events;

namespace Mediator;

public class MediatorActionConsumer : IConsumer<IAcceptMediatorAction>
{
    public Task Consume(ConsumeContext<IAcceptMediatorAction> context)
    {
        return context.RespondAsync<IMediatorActionAccepted>(new
        {
            Text = $"Mediator action accepted: {context.Message.Id} - {DateTime.UtcNow}"
        });
    }
}