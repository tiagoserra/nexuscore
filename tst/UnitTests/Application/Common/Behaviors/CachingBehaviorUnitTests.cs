using Application.Common.Attributes;
using Application.Common.Behaviors;
using Application.Common.Commands;
using Application.Common.Enums;
using Application.Common.Interfaces;
using MediatR;
using Moq;
using Newtonsoft.Json;

namespace UnitTests.Application.Common.Behaviors;

public class CachingBehaviorUnitTests
{
    private readonly Mock<ICache> _cacheMock = new Mock<ICache>();
    private readonly Mock<IExecutionContext> _executionContextMock = new Mock<IExecutionContext>();

    [Fact]
    public async Task Handle_NoCacheAttribute_ShouldCallNext()
    {
        // Arrange
        var request = new RequestWithoutCacheAttribute();
        var behavior = new CachingBehavior<RequestWithoutCacheAttribute, ResponseCommand>(_cacheMock.Object, _executionContextMock.Object);

        // Act
        var result = await behavior.Handle(request, () => Task.FromResult(new ResponseCommand(ResponseStatusCommand.Ok)), CancellationToken.None);

        // Assert
        _cacheMock.Verify(cache => cache.GetAsync<string>(It.IsAny<string>()), Times.Never);
        _cacheMock.Verify(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        Assert.IsType<ResponseCommand>(result);
    }

    [Fact]
    public async Task Handle_CacheAttribute_ShouldCacheResponse()
    {
        // Arrange
        var request = new RequestWithCacheAttribute();
        var response = new ResponseCommand(ResponseStatusCommand.Ok);
        var cacheKey = $"Cache:{request.GetType().Name}:{request.GetHashCode()}";
        var behavior = new CachingBehavior<RequestWithCacheAttribute, ResponseCommand>(_cacheMock.Object, _executionContextMock.Object);

        _cacheMock.Setup(cache => cache.GetAsync<string>(cacheKey)).ReturnsAsync("");
        _cacheMock.Setup(cache => cache.SetAsync(cacheKey, It.IsAny<string>(), It.IsAny<int>())).Returns(Task.CompletedTask);

        // Act
        var result = await behavior.Handle(request, () => Task.FromResult(response), CancellationToken.None);

        // Assert
        _cacheMock.Verify(cache => cache.GetAsync<string>(cacheKey), Times.Once);
        _cacheMock.Verify(cache => cache.SetAsync(cacheKey, JsonConvert.SerializeObject(response), It.IsAny<int>()), Times.Once);
        Assert.Equal(response, result);
    }
}

public class RequestWithoutCacheAttribute : IRequest<ResponseCommand> { }

[CacheAttribute(durationInMinutes: 10)]
public class RequestWithCacheAttribute : IRequest<ResponseCommand> { }