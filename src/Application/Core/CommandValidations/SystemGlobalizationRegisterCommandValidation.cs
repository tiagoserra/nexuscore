using Application.Core.Commands;
using Microsoft.Extensions.Localization;
using FluentValidation;

namespace Application.Core.CommandValidations;

public class SystemGlobalizationRegisterCommandValidation : AbstractValidator<SystemGlobalizationRegisterCommand>
{
    public SystemGlobalizationRegisterCommandValidation(IStringLocalizer localizer)
    {
        RuleFor(v => v.Key)
            .MaximumLength(255).WithMessage(string.Format(localizer["Common:Message:Error:MinMaxLengthCustom"].Value, localizer["Common:Label:key"].Value, "1", "255"))
            .NotEmpty().WithMessage(string.Format(localizer["Common:Message:Required:Field"].Value, localizer["Common:Label:key"].Value));

        RuleFor(v => v.Resource)
            .NotNull().WithMessage(string.Format(localizer["Common:Message:Required:Field"].Value, localizer["Common:Label:Resource"].Value));
    }
}