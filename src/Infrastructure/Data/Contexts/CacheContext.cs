using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Application.Common.Interfaces;
using Newtonsoft.Json;

namespace Infrastructure.Data.Contexts;

public class CacheContext : ICache
{
    readonly IDistributedCache _cache = null;

    public CacheContext(IDistributedCache cache) 
        => _cache = cache;
    
    public async Task<T> GetAsync<T>(string key)
    {
        var result = await _cache.GetStringAsync(key);

        if (!string.IsNullOrWhiteSpace(result))
            return JsonConvert.DeserializeObject<T>(result);

        return default;
    }

    public async Task RemoveAsync(string key)
        => await _cache.RemoveAsync(key);

    public async Task SetAsync(string key, object value)
        => await _cache.SetAsync(key, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)));

    public async Task SetAsync(string key, object value, int ttl)
        => await _cache.SetAsync(key, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(ttl) });
}