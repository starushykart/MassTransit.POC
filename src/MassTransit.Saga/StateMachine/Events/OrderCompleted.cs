namespace MassTransit.Saga.StateMachine.Events;

public record OrderCompleted
{
    public Guid OrderId { get; init; }
}