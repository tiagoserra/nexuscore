using Application.Core.Commands;
using Microsoft.Extensions.Localization;
using FluentValidation;

namespace Application.Core.CommandValidations;

public class SystemGlobalizationAlterCommandValidation : AbstractValidator<SystemGlobalizationAlterCommand>
{
    public SystemGlobalizationAlterCommandValidation(IStringLocalizer localizer)
    {
        RuleFor(p => p.Id)
           .NotNull().WithMessage(string.Format(localizer["Common:Message:Required:Field"], "Id"));

        RuleFor(v => v.Resource)
            .NotNull().WithMessage(string.Format(localizer["Common:Message:Required:Field"].Value, localizer["Common:Label:Resource"].Value));
    }
}