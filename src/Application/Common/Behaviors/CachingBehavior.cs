using System.Reflection;
using Application.Common.Attributes;
using Application.Common.Commands;
using Application.Common.Interfaces;
using MediatR;
using Newtonsoft.Json;

namespace Application.Common.Behaviors;

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResponseCommand
{

    private readonly ICache _cache;
    private readonly IExecutionContext _executionContext;

    public CachingBehavior(ICache cache, IExecutionContext executionContext)
    {
        _cache = cache;
        _executionContext = executionContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var cacheAttribute = request.GetType().GetCustomAttribute<CacheAttribute>();

        if (cacheAttribute == null)
            return await next();

        var cacheKey = $"Cache:{request.GetType().Name}:{request.GetHashCode()}";

        if (cacheAttribute.CachePerUser)
            cacheKey += $":User:{_executionContext.ExecutionId}";

        var cachedResult = await _cache.GetAsync<string>(cacheKey);

        if (!string.IsNullOrEmpty(cachedResult))
            return JsonConvert.DeserializeObject<TResponse>(cachedResult);

        var response = await next();
        await _cache.SetAsync(cacheKey, JsonConvert.SerializeObject(response), cacheAttribute.DurationInMinutes);

        return response;
    }
}
