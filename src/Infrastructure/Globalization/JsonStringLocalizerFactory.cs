using Microsoft.Extensions.Localization;

namespace Infrastructure.Globalization;

public class JsonStringLocalizerFactory : IStringLocalizerFactory
{
    private readonly IStringLocalizer _stringLocalizer;

    public JsonStringLocalizerFactory(IStringLocalizer stringLocalizer) 
        => _stringLocalizer = stringLocalizer;

    public IStringLocalizer Create(Type resourceSource) 
        => _stringLocalizer;

    public IStringLocalizer Create(string baseName, string location) 
        => _stringLocalizer;
}