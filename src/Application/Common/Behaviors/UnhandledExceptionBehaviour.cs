using Microsoft.Extensions.Logging;
using Application.Common.Commands;
using Domain.Common.Exceptions;
using MediatR;

namespace Application.Common.Behaviors;

public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResponseCommand
{
    private readonly ILogger<TRequest> _logger;

    public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (DomainException domainException)
        {
            _logger.LogError($"Request: Unhandled Exception type: {domainException.GetType().Name} - {request}");
            throw;
        }
        catch (Exception exception)
        {
            _logger.LogError($"Request: Unhandled Exception type: {exception.GetType().Name} - {request}");
            throw;
        }
    }
}