using Microsoft.Extensions.Localization;
using Application.Common.Commands;
using Application.Common.Enums;
using Application.Core.Commands;
using Application.Core.Interfaces;
using Domain.Core.Entities;
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
        SystemGlobalization systemglobalization = await _repository.GetByIdAsync(request.Id);

        if(systemglobalization is null)
            return new ResponseCommand(ResponseStatusCommand.NotFound);

        systemglobalization.AlterTranlastion(request.Resource);

        //todo event update cache

        await _repository.UpdateAsync(systemglobalization);

        return new ResponseCommand(systemglobalization);
    }
}