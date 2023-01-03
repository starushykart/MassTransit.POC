namespace MassTransit.Saga.StateMachine.Events;

public record OrderCancelled
{
    public Guid OrderId { get; init; }
}