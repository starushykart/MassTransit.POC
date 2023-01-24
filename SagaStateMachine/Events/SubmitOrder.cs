namespace SagaStateMachine.Events;

public record SubmitOrder
{
    public Guid OrderId { get; init; }
}