using Application.Common.Attributes;
using Application.Common.Commands;
using Domain.Core.Permissions;
using MediatR;

namespace Application.Core.CommandQueries;

[AuthorizeAttribute(Role = SystemGlobalizationPermission.Role, Policy = SystemGlobalizationPermission.Policy.Read)]
public class SystemGlobalizationGetByIdCommandQuery : IRequest<ResponseCommand>
{
    public long Id { get; }

    public SystemGlobalizationGetByIdCommandQuery(long id) => Id = id;
}