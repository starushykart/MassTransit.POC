using MassTransit;

namespace SagaConsumer.Events;

public record SubmitExport : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; init; }
}