namespace SagaStateMachine.StateMachine.Events;

public record OrderCancelled
{
    public Guid OrderId { get; init; }
}