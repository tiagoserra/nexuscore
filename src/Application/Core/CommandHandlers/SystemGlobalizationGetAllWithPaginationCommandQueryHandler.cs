using Microsoft.Extensions.Localization;
using Application.Common.Commands;
using Application.Common.Enums;
using Application.Core.CommandQueries;
using Application.Core.Interfaces;
using MediatR;

namespace Application.Core.CommandHandlers;

public class SystemGlobalizationGetAllWithPaginationCommandQueryHandler : IRequestHandler<SystemGlobalizationGetAllWithPaginationCommandQuery, ResponseCommand>
{
    private readonly IStringLocalizer _localizer;
    private readonly ISystemGlobalizationRepository _repository;

    public SystemGlobalizationGetAllWithPaginationCommandQueryHandler(IStringLocalizer localizer, ISystemGlobalizationRepository repository)
    {
        _localizer = localizer;
        _repository = repository;
    }

    public async Task<ResponseCommand> Handle(SystemGlobalizationGetAllWithPaginationCommandQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByPaginatedAsync(request.PageNumber, request.PageSize);

        if(result is null)
            return new ResponseCommand(ResponseStatusCommand.NotFound);

        return new ResponseCommand(result);
    }
}