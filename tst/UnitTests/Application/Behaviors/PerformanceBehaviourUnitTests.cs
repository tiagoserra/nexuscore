using Microsoft.Extensions.Logging;
using Application.Common.Behaviors;
using Application.Common.Commands;
using MediatR;
using Moq;
using Xunit.Abstractions;

namespace UnitTests.Application.Behaviors;

public class PerformanceBehaviourUnitTests
{
    [Fact]
    public async Task Handle_ShouldLogWarningWhenElapsedTimeIsGreaterThan500Milliseconds()
    {
        // Arrange
        var logger = new TestLogger<SampleRequest>();
        var request = new SampleRequest();
        var requestHandlerDelegateMock = new Mock<RequestHandlerDelegate<ResponseCommand>>();
        requestHandlerDelegateMock
            .Setup(x => x())
            .ReturnsAsync(new ResponseCommand(new { Success = true }))
            .Callback(() => Thread.Sleep(600)); // Simulate a request that takes longer than 500ms

        var performanceBehaviour = new PerformanceBehaviour<SampleRequest, ResponseCommand>(logger);

        // Act
        var response = await performanceBehaviour.Handle(request, requestHandlerDelegateMock.Object, CancellationToken.None);

        // Assert
        Assert.True(response.ConvertTo<dynamic>().Success);
        Assert.Contains(logger.LoggedMessages, message => message.Contains("PerformanceBehaviour Long Running Request"));
    }

    [Fact]
    public async Task Handle_ShouldLogWarningWhenElapsedTimeIsLessThan500Milliseconds()
    {
        // Arrange
        var logger = new TestLogger<SampleRequest>();
        var request = new SampleRequest();
        var requestHandlerDelegateMock = new Mock<RequestHandlerDelegate<ResponseCommand>>();
        requestHandlerDelegateMock
            .Setup(x => x())
            .ReturnsAsync(new ResponseCommand(new { Success = true }))
            .Callback(() => Thread.Sleep(100)); // Simulate a request that takes less than 100ms

        var performanceBehaviour = new PerformanceBehaviour<SampleRequest, ResponseCommand>(logger);

        // Act
        var response = await performanceBehaviour.Handle(request, requestHandlerDelegateMock.Object, CancellationToken.None);

        // Assert
        Assert.True(response.ConvertTo<dynamic>().Success);
        Assert.DoesNotContain(logger.LoggedMessages, message => message.Contains("PerformanceBehaviour Long Running Request"));
    }
}

public class SampleRequest : IRequest<ResponseCommand> { }

public class TestLogger<T> : ILogger<T>
{
    public List<string> LoggedMessages { get; } = new List<string>();

    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (IsEnabled(logLevel))
        {
            LoggedMessages.Add(formatter(state, exception));
        }
    }
}