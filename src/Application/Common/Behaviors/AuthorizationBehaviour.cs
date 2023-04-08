using System.Reflection;
using Application.Common.Attributes;
using Application.Common.Commands;
using Application.Common.Enums;
using Application.Common.Interfaces;
using Domain.Common.Permissions;
using MediatR;

namespace Application.Common.Behaviors;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResponseCommand
{
    private IExecutionContext _executionContext;

    public AuthorizationBehaviour(IExecutionContext executionContext)
    {
        _executionContext = executionContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_executionContext.HasRole(SysAdminPermission.Role))
        {
            var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

            var response = new ResponseCommand(ResponseStatusCommand.Forbidden);
            response.AddError($"{(int)SystemErrorType.CommandError}", $"Forbidden access");

            if (authorizeAttributes.Any())
            {
                var authorizeAttributesWithRoles = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Role));

                if (authorizeAttributesWithRoles.Any())
                {
                    var authorized = false;

                    foreach (var flags in authorizeAttributesWithRoles.Select(a => a.Role.Split(',')))
                    {
                        foreach (var role in flags)
                        {
                            if (_executionContext.HasRole(role.Trim()))
                            {
                                authorized = true;
                                break;
                            }
                        }
                    }

                    if (!authorized)
                        return await Task.FromResult(response as TResponse);

                    var authorizeAttributesWithPolicies = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy));

                    if (authorizeAttributesWithPolicies.Any())
                    {
                        foreach (var policy in authorizeAttributesWithPolicies.Select(a => a.Policy))
                        {
                            if (!_executionContext.HasPolicy(policy))
                                return await Task.FromResult(response as TResponse);
                        }
                    }
                }
            }
        }

        return await next();
    }
}