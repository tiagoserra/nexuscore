using Domain.Common.Entities;
using Domain.Common.Exceptions;

namespace Domain.Core.Entities;

public class SystemGlobalization : Entity
{
    public string Key { get; private set; }
    public Dictionary<string, string> Resource { get; private set; }

    public SystemGlobalization(string key, Dictionary<string, string> resource)
    {
        DomainException.When(string.IsNullOrEmpty(key), "key", "Common:Message:Required:Field");
        DomainException.When((resource is null || !resource.Any()), "resource", "Common:Message:Required:Field");

        Key = key.ToLower();
        Resource = resource;
    }

    protected SystemGlobalization() { }

    public void AlterTranlastion(string language, string item)
    {
        DomainException.When((Resource is null || !Resource.ContainsKey(language)), "language", "Common:Message:Error:Value:NotFound");

        Resource[language] = item;
    }

    public void AlterTranlastion(Dictionary<string, string> resource)
    {
        DomainException.When(resource is null, "resource", "Common:Message:Required:Field");

        Resource = resource;
    }
}