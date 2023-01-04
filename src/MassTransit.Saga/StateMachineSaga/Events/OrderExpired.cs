namespace MassTransit.Saga.StateMachineSaga.Events;

public record OrderExpired
{
    public Guid OrderId { get; init; }
}