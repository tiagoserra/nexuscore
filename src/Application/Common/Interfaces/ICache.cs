namespace Application.Common.Interfaces;

public interface ICache
{
    Task<T> GetAsync<T>(string key);

    Task SetAsync(string key, object value);

    Task SetAsync(string key, object value, int ttl);

    Task RemoveAsync(string key);
}