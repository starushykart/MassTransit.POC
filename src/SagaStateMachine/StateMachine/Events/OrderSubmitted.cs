namespace SagaStateMachine.StateMachine.Events;

public record OrderSubmitted
{
    public Guid OrderId { get; init; }
}