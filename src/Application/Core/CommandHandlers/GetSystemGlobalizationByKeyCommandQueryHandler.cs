using Microsoft.Extensions.Localization;
using Application.Common.Commands;
using Application.Common.Enums;
using Application.Core.CommandQueries;
using Domain.Core.Interfaces;
using MediatR;

namespace Application.Core.CommandHandlers;

public class GetSystemGlobalizationByKeyCommandQueryHandler : IRequestHandler<GetSystemGlobalizationByKeyCommandQuery, ResponseCommand>
{
    private readonly IStringLocalizer _localizer;
    private readonly ISystemGlobalizationRepository _repository;

    public GetSystemGlobalizationByKeyCommandQueryHandler(IStringLocalizer localizer, ISystemGlobalizationRepository repository)
    {
        _localizer = localizer;
        _repository = repository;
    }

    public async Task<ResponseCommand> Handle(GetSystemGlobalizationByKeyCommandQuery request, CancellationToken cancellationToken)
    {
        var systemglobalization = await _repository.GetByKeyAsync(request.Key);

        return systemglobalization is null ? new ResponseCommand(ResponseStatusCommand.NotFound) : new ResponseCommand(systemglobalization);
    }
}