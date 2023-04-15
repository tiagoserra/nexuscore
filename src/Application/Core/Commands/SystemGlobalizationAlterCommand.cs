using Application.Common.Attributes;
using Application.Common.Commands;
using Domain.Core.Permissions;
using MediatR;

namespace Application.Core.Commands;

[AuthorizeAttribute(Role = SystemGlobalizationPermission.Role, Policy = SystemGlobalizationPermission.Policy.Write)]
public class SystemGlobalizationAlterCommand : IRequest<ResponseCommand>
{
    public string Key { get; }
    public Dictionary<string, string> Resource { get; }

    public SystemGlobalizationAlterCommand(string key, Dictionary<string, string> resource)
    {
        Key = key;
        Resource = resource;
    }
}