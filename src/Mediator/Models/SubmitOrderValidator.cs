using FluentValidation;

namespace Mediator.Models;

public class SubmitOrderValidator : AbstractValidator<ISubmitOrder>
{
    public SubmitOrderValidator()
    {
        RuleFor(x => x.Date)
            .LessThanOrEqualTo(DateTime.UtcNow);
    }
}