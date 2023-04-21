using System.Reflection;
using System.Text.Json;
using Application.Common.Attributes;
using Application.Common.Commands;
using Application.Common.Interfaces;
using Domain.Core.Entities;
using Domain.Core.Interfaces;
using MediatR;

namespace Application.Common.Behaviors;

public class AuditBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResponseCommand
{
    private readonly IExecutionContext _executionContext;
    private readonly IAuditRepository _auditRepository;

    public AuditBehaviour(IExecutionContext executionContext, IAuditRepository auditRepository)
    {
        _executionContext = executionContext;
        _auditRepository = auditRepository;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var attribute = request.GetType().GetCustomAttribute<AuditAttribute>();

        if (attribute is null || (attribute != null && !attribute.IsAuditable))
            return await next();

        var executionTime = DateTime.Now;

        TResponse response = await next();

        var audit = new Audit(
                _executionContext.ExecutionContextId,
                JsonSerializer.Serialize(request),
                response == null ? null : JsonSerializer.Serialize(response),
                DateTimeOffset.UtcNow - executionTime,
                typeof(TRequest).Name
            );

        await _auditRepository.InsertAsync(audit);

        return response;
    }
}