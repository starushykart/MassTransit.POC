namespace MassTransit.Saga.StateMachineSaga.Events;

public record OrderCancelled
{
    public Guid OrderId { get; init; }
}