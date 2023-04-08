using Application.Common.Commands;
using Application.Common.Enums;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.Common.Behaviors;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResponseCommand
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        => _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var failures = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(f => f is not null)
                .ToList();

            if (failures.Count != 0)
                return await Errors(failures);
        }

        return await next();
    }

    private static async Task<TResponse> Errors(IEnumerable<ValidationFailure> failures)
    {
        var response = new ResponseCommand(ResponseStatusCommand.Error);

        foreach (var failure in failures)
            response.AddError($"{(int)SystemErrorType.CommandError} - {failure.PropertyName}", $"{failure.PropertyName} : {failure.ErrorMessage}");

        return await Task.FromResult(response as TResponse);
    }
}