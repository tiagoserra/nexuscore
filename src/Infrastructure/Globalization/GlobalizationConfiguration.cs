using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Infrastructure.Globalization;

public static class GlobalizationConfiguration
{
    public static IServiceCollection AddGlobalization(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");
        services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
        services.AddSingleton<IStringLocalizer, JsonStringLocalizer>();

        return services;
    }

    public static IApplicationBuilder AddGlobalization(this IApplicationBuilder app, string language)
    {
        var supportedCultures = new[] { new CultureInfo("pt-BR"), new CultureInfo("en-US") };

        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(GetLanguage(language)),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures,
            RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider()
                }
        });

        return app;
    }

    public static CultureInfo GetLanguage(string language)
    {
        CultureInfo cultureInfo;

        switch (language)
        {
            case "pt-BR":
            case "Portuguese":
            default:
                cultureInfo = new("pt-BR");

                cultureInfo.NumberFormat.CurrencySymbol = "R$";
                cultureInfo.NumberFormat.CurrencyGroupSeparator = ",";

                break;
            case "en-US":
            case "English":
                cultureInfo = new("en-US");

                cultureInfo.NumberFormat.CurrencySymbol = "$";
                cultureInfo.NumberFormat.CurrencyGroupSeparator = ".";

                break;
        }

        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

        return cultureInfo;
    }
}