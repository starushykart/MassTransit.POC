using FluentValidation;

namespace SagaStateMachine.Mediator.Models;

public class SubmitOrderValidator : AbstractValidator<ISubmitOrder>
{
    public SubmitOrderValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
    }
}