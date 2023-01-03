namespace MassTransit.Saga.StateMachine;

public class OrderState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    public string CurrentState { get; set; } = default!;
    
    public DateTime? CancelledAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Guid? ExpirationToken { get; set; }
    public int ReadyEventStatus { get; set; }
}