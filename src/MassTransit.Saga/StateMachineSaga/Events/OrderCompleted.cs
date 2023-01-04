namespace MassTransit.Saga.StateMachineSaga.Events;

public record OrderCompleted
{
    public Guid OrderId { get; init; }
}