using Microsoft.Extensions.Localization;
using Application.Common.Commands;
using Application.Common.Enums;
using Application.Core.Commands;
using Domain.Core.Interfaces;
using MediatR;

namespace Application.Core.CommandHandlers;

public class SystemGlobalizationAlterCommandHandler : IRequestHandler<SystemGlobalizationAlterCommand, ResponseCommand>
{
    private readonly IStringLocalizer _localizer;
    private readonly ISystemGlobalizationRepository _repository;

    public SystemGlobalizationAlterCommandHandler(IStringLocalizer localizer, ISystemGlobalizationRepository repository)
    {
        _localizer = localizer;
        _repository = repository;
    }

    public async Task<ResponseCommand> Handle(SystemGlobalizationAlterCommand request, CancellationToken cancellationToken)
    {
        var systemglobalization = await _repository.GetByKeyAsync(request.Key);

        if(systemglobalization is null)
            return new ResponseCommand(ResponseStatusCommand.NotFound);

        systemglobalization.AlterTranlastion(request.Resource);

        //todo event update cache

        await _repository.UpdateAsync(systemglobalization);

        return new ResponseCommand(systemglobalization);
    }
}