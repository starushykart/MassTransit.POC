namespace SagaStateMachine.Events;

public record OrderProcessing
{
    public Guid OrderId { get; init; }
}