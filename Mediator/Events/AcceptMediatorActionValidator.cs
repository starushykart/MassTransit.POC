using FluentValidation;

namespace Mediator.Events;

public class AcceptMediatorActionValidator : AbstractValidator<IAcceptMediatorAction>
{
    public AcceptMediatorActionValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}