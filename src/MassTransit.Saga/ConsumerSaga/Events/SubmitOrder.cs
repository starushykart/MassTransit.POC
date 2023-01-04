namespace MassTransit.Saga.StateMachineSaga.Events;

public record SubmitExport : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; init; }
}