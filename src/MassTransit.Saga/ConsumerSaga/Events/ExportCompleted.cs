namespace MassTransit.Saga.StateMachineSaga.Events;

public record ExportCompleted : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; init; }
}