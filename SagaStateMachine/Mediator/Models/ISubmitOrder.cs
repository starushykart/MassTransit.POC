namespace SagaStateMachine.Mediator.Models;

public interface ISubmitOrder
{
    Guid? OrderId { get; }
}