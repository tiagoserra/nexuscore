using Application.Common.Attributes;
using Application.Common.Commands;
using Domain.Core.Permissions;
using MediatR;

namespace Application.Core.CommandQueries;

[AuthorizeAttribute(Role = SystemGlobalizationPermission.Role, Policy = SystemGlobalizationPermission.Policy.Read)]
public class SystemGlobalizationGetAllWithPaginationCommandQuery : IRequest<ResponseCommand>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}