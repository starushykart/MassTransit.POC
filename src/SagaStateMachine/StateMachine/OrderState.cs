using Marten.Schema;
using MassTransit;

namespace SagaStateMachine.StateMachine;

public class OrderState : SagaStateMachineInstance
{
    [Identity]
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; } = default!;
    
    public DateTime? SubmittedAt { get; set; }
    public DateTime? ExpiredAt { get; set; }
    public Guid? ExpirationToken { get; set; }
    public int ReadyEventStatus { get; set; }
}