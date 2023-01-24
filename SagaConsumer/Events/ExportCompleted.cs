using MassTransit;

namespace SagaConsumer.Events;

public record ExportCompleted : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; init; }
}