namespace SagaStateMachine.StateMachine.Events;

public record OrderExpired
{
    public Guid OrderId { get; init; }
}