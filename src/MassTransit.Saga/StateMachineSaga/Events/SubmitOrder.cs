namespace MassTransit.Saga.StateMachineSaga.Events;

public record SubmitOrder
{
    public Guid OrderId { get; init; }
}