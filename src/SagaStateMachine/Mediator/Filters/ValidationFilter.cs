using FluentValidation;
using MassTransit;

namespace SagaStateMachine.Mediator.Filters;

public class ValidationFilter<T> : IFilter<SendContext<T>>
    where T : class
{
    private readonly IValidator<T> _validator;

    public ValidationFilter(IValidator<T> validator = null)
    {
        _validator = validator;
    }

    public Task Send(SendContext<T> context, IPipe<SendContext<T>> next)
    {
        _validator?.ValidateAndThrow(context.Message);

        return next.Send(context);
    }

    public void Probe(ProbeContext context)
    { }
}