using Microsoft.Extensions.Localization;
using Application.Common.Commands;
using Application.Common.Enums;
using Application.Core.CommandQueries;
using Application.Core.Interfaces;
using Domain.Core.Entities;
using MediatR;

namespace Application.Core.CommandHandlers;

public class SystemGlobalizationGetByIdCommandQueryHandler : IRequestHandler<SystemGlobalizationGetByIdCommandQuery, ResponseCommand>
{
    private readonly IStringLocalizer _localizer;
    private readonly ISystemGlobalizationRepository _repository;

    public SystemGlobalizationGetByIdCommandQueryHandler(IStringLocalizer localizer, ISystemGlobalizationRepository repository)
    {
        _localizer = localizer;
        _repository = repository;
    }

    public async Task<ResponseCommand> Handle(SystemGlobalizationGetByIdCommandQuery request, CancellationToken cancellationToken)
    {
        SystemGlobalization systemglobalization = await _repository.GetByIdAsync(request.Id);

        if(systemglobalization is null)
            return new ResponseCommand(ResponseStatusCommand.NotFound);

        return new ResponseCommand(systemglobalization);
    }
}