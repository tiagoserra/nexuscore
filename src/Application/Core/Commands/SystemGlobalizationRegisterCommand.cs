using Application.Common.Attributes;
using Application.Common.Commands;
using Domain.Core.Permissions;
using MediatR;

namespace Application.Core.Commands;

[AuthorizeAttribute(Role = SystemGlobalizationPermission.Role, Policy = SystemGlobalizationPermission.Policy.Write)]
public class SystemGlobalizationRegisterCommand : IRequest<ResponseCommand>
{
    public string Key { get; set; }
    public Dictionary<string, string> Resource { get; set; }


    public SystemGlobalizationRegisterCommand(string key, Dictionary<string, string> resource)
    {
        Key = key;
        Resource = resource;
    }
}