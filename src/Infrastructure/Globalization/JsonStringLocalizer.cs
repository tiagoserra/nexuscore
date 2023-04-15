using System.Globalization;
using Application.Common.Interfaces;
using Application.Core.Dtos;
using Application.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Infrastructure.Globalization;

public class JsonStringLocalizer : IStringLocalizer
{
    private readonly List<SystemGlobalizationDto> localization;

    private readonly ISystemGlobalizationRepository _repository;

    readonly ICache _cache;

    public JsonStringLocalizer(IServiceScopeFactory _scopeFactory, ICache cache, IServiceScopeFactory scope)
    {
        _scopeFactory = scope;
        _repository = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ISystemGlobalizationRepository>(); //gbt do Ioc pqp

        _cache = cache;

        var cacheResource = _cache.GetAsync<List<SystemGlobalizationDto>>("globalization").GetAwaiter().GetResult();

        if (cacheResource is null)
        {
            var resources = _repository.GetAllAsync().GetAwaiter().GetResult();

            if (resources is not null)
            {
                _cache.SetAsync("globalization", resources).GetAwaiter().GetResult();
                localization = resources.Select(p => new SystemGlobalizationDto(p.Key, p.Resource)).ToList();
            }
        }
        else
            localization = cacheResource;
    }

    public LocalizedString this[string name]
    {
        get
        {
            var value = GetString(name);
            return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var format = GetString(name);
            var value = string.Format(format ?? name, arguments);
            return new LocalizedString(name, value, resourceNotFound: format == null);
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        return localization.Where(l => l.Resource.Keys.Any(lv => lv == CultureInfo.CurrentCulture.Name)).Select(l => new LocalizedString(l.Key ?? "", l.Resource[CultureInfo.CurrentCulture.Name], true));
    }

    private string GetString(string name)
    {
        var query = localization.Where(l => l.Resource.Keys.Any(lv => lv == CultureInfo.CurrentCulture.Name));
        var value = query.FirstOrDefault(l => l.Key == name);

        return value is not null ? value.Resource[CultureInfo.CurrentCulture.Name] : name;
    }
}
