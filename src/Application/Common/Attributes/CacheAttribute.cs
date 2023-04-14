namespace Application.Common.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class CacheAttribute : Attribute
{
    public int DurationInMinutes { get; }
    public bool CachePerUser { get; }
    
    public CacheAttribute(int durationInMinutes, bool cachePerUser = false)
    {
        DurationInMinutes = durationInMinutes;
        CachePerUser = cachePerUser;
    }
}