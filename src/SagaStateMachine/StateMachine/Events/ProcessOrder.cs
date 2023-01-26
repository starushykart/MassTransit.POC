namespace SagaStateMachine.StateMachine.Events;

public record ProcessOrder (Guid OrderId, bool ApplyDiscount);