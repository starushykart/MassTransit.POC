namespace MassTransit.Saga.StateMachine.Events;

public record OrderProcessing
{
    public Guid OrderId { get; init; }
}