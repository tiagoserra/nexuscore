using Microsoft.Extensions.Localization;
using Application.Common.Commands;
using Application.Common.Enums;
using Application.Core.Commands;
using Domain.Core.Interfaces;
using Domain.Core.Entities;
using MediatR;

namespace Application.Core.CommandHandlers;

public class SystemGlobalizationRegisterCommandHandler : IRequestHandler<SystemGlobalizationRegisterCommand, ResponseCommand>
{
    private readonly IStringLocalizer _localizer;
    private readonly ISystemGlobalizationRepository _repository;

    public SystemGlobalizationRegisterCommandHandler(IStringLocalizer localizer, ISystemGlobalizationRepository repository)
    {
        _localizer = localizer;
        _repository = repository;
    }

    public async Task<ResponseCommand> Handle(SystemGlobalizationRegisterCommand request, CancellationToken cancellationToken)
    {
        var systemglobalization = await _repository.GetByKeyAsync(request.Key.ToLower());

        if (systemglobalization is not null)
        {
            var response = new ResponseCommand(ResponseStatusCommand.Error);
            response.AddError("Common:Message:Error:AlreadyExists", string.Format(_localizer["Common:Message:Error:AlreadyExists"].Value, request.Key));

            return response;
        }

        systemglobalization = new SystemGlobalization(request.Key, request.Resource);

        await _repository.InsertAsync(systemglobalization);

        return new ResponseCommand(systemglobalization.Id);
    }
}