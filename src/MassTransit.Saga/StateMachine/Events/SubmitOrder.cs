namespace MassTransit.Saga.StateMachine.Events;

public record SubmitOrder
{
    public Guid OrderId { get; init; }
}