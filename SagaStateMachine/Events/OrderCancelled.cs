namespace SagaStateMachine.Events;

public record OrderCancelled
{
    public Guid OrderId { get; init; }
}