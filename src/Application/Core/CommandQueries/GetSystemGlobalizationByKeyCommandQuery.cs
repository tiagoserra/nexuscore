using Application.Common.Attributes;
using Application.Common.Commands;
using Domain.Core.Permissions;
using MediatR;

namespace Application.Core.CommandQueries;

[AuthorizeAttribute(Role = SystemGlobalizationPermission.Role, Policy = SystemGlobalizationPermission.Policy.Read)]

public class GetSystemGlobalizationByKeyCommandQuery: IRequest<ResponseCommand>
{
    public string Key { get; set; }

    public GetSystemGlobalizationByKeyCommandQuery(string key) 
        => Key = key;
}
