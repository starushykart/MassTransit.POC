namespace SagaStateMachine.Events;

public record OrderExpired
{
    public Guid OrderId { get; init; }
}