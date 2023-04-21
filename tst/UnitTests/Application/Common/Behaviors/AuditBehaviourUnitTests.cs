using Application.Common.Attributes;
using Application.Common.Behaviors;
using Application.Common.Commands;
using Application.Common.Enums;
using Application.Common.Interfaces;
using Domain.Core.Entities;
using Domain.Core.Interfaces;
using MediatR;
using Moq;

namespace UnitTests.Application.Common.Behaviors;

public class AuditBehaviourUnitTests
{
    [Fact]
    public async Task Handle_WithAuditableRequest_AuditsRequestAndResponse()
    {
        // Arrange
        var mockExecutionContext = new Mock<IExecutionContext>();
        var mockAuditRepository = new Mock<IAuditRepository>();

        var auditBehaviour = new AuditBehaviour<AuditableRequest, ResponseCommand>(mockExecutionContext.Object, mockAuditRepository.Object);

        var request = new AuditableRequest();
        var cancellationToken = new CancellationToken();
        RequestHandlerDelegate<ResponseCommand> next = () => Task.FromResult(new ResponseCommand(ResponseStatusCommand.Ok));

        // Act
        var response = await auditBehaviour.Handle(request, next, cancellationToken);

        // Assert
        mockAuditRepository.Verify(repo => repo.InsertAsync(It.IsAny<Audit>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonAuditableRequest_DoesNotAuditRequestAndResponse()
    {
        // Arrange
        var mockExecutionContext = new Mock<IExecutionContext>();
        var mockAuditRepository = new Mock<IAuditRepository>();

        var auditBehaviour = new AuditBehaviour<NonAuditableRequest, ResponseCommand>(mockExecutionContext.Object, mockAuditRepository.Object);

        var request = new NonAuditableRequest();
        var cancellationToken = new CancellationToken();
        RequestHandlerDelegate<ResponseCommand> next = () => Task.FromResult(new ResponseCommand(ResponseStatusCommand.Ok));

        // Act
        var response = await auditBehaviour.Handle(request, next, cancellationToken);

        // Assert
        mockAuditRepository.Verify(repo => repo.InsertAsync(It.IsAny<Audit>()), Times.Never);
    }
}
[AuditAttribute(true)]
public class AuditableRequest : IRequest<ResponseCommand> { }

public class NonAuditableRequest : IRequest<ResponseCommand>  { }