using Application.Common.Behaviors;
using Application.Common.Commands;
using Domain.Common.Exceptions;
using MediatR;
using Moq;

namespace UnitTests.Application.Behaviors;

public class UnhandledExceptionBehaviourUnitTests
{
    [Fact]
    public async Task Handle_ShouldCatchAndLogDomainException()
    {
        // Arrange
        var logger = new TestLogger<SampleRequest>();
        var request = new SampleRequest();
        var requestHandlerDelegateMock = new Mock<RequestHandlerDelegate<ResponseCommand>>();
        requestHandlerDelegateMock
            .Setup(x => x())
            .ThrowsAsync(new DomainException("Test domain exception"));

        var unhandledExceptionBehaviour = new UnhandledExceptionBehaviour<SampleRequest, ResponseCommand>(logger);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => unhandledExceptionBehaviour.Handle(request, requestHandlerDelegateMock.Object, CancellationToken.None));
        Assert.Contains(logger.LoggedMessages, message => message.Contains("Unhandled Exception type: DomainException"));
    }

    [Fact]
    public async Task Handle_ShouldCatchAndLogGeneralException()
    {
        // Arrange
        var logger = new TestLogger<SampleRequest>();
        var request = new SampleRequest();
        var requestHandlerDelegateMock = new Mock<RequestHandlerDelegate<ResponseCommand>>();
        requestHandlerDelegateMock
            .Setup(x => x())
            .ThrowsAsync(new InvalidOperationException("Test general exception"));

        var unhandledExceptionBehaviour = new UnhandledExceptionBehaviour<SampleRequest, ResponseCommand>(logger);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => unhandledExceptionBehaviour.Handle(request, requestHandlerDelegateMock.Object, CancellationToken.None));
        Assert.Contains(logger.LoggedMessages, message => message.Contains("Unhandled Exception type: InvalidOperationException"));
    }
}