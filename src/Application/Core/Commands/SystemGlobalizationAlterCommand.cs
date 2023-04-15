using Application.Common.Attributes;
using Application.Common.Commands;
using Domain.Core.Permissions;
using MediatR;

namespace Application.Core.Commands;

[AuthorizeAttribute(Role = SystemGlobalizationPermission.Role, Policy = SystemGlobalizationPermission.Policy.Write)]
public class SystemGlobalizationAlterCommand : IRequest<ResponseCommand>
{
    public long Id { get; set; }
    public Dictionary<string, string> Resource { get; set; }

    public SystemGlobalizationAlterCommand(long id, Dictionary<string, string> resource)
    {
        Id = id;
        Resource = resource;
    }
}