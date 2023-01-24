namespace SagaStateMachine.Events;

public record OrderCompleted
{
    public Guid OrderId { get; init; }
}