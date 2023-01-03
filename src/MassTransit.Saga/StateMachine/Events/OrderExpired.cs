namespace MassTransit.Saga.StateMachine.Events;

public record OrderExpired
{
    public Guid OrderId { get; init; }
}