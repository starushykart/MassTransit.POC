namespace MassTransit.Saga.StateMachineSaga.Events;

public record OrderProcessing
{
    public Guid OrderId { get; init; }
}