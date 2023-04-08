using Application.Common.Attributes;
using Application.Common.Behaviors;
using Application.Common.Commands;
using Application.Common.Enums;
using Application.Common.Interfaces;
using Domain.Common.Permissions;
using MediatR;
using Moq;

namespace UnitTests.Application.Common.Behaviors;

[AuthorizeAttribute(Role = "role", Policy = "policy")]
public class CommandTestPermission : IRequest<ResponseCommand>
{
    public virtual string SomeProperty { get; set; }
}

public class AuthorizationBehaviourTest
{
    [Fact]
    public async Task Handle_WithAuthorizationAttributeAndMatchingRole_ShouldCallNext_SysAdminRole()
    {
        //Arrange
        var executionContextMock = new Mock<IExecutionContext>();
        executionContextMock.Setup(e => e.HasRole(SysAdminPermission.Role)).Returns(true);

        var authorizeAttribute = new AuthorizeAttribute();

        var request = new CommandTestPermission();
        request.SomeProperty = "SomeValue";

        var requestMock = new Mock<CommandTestPermission>();
        requestMock.SetupGet(r => r.SomeProperty).Returns(request.SomeProperty);

        var nextMock = new Mock<RequestHandlerDelegate<ResponseCommand>>();
        nextMock.Setup(m => m.Invoke()).ReturnsAsync(new ResponseCommand(ResponseStatusCommand.Forbidden));
        var cancellationToken = new CancellationToken();

        var behavior = new AuthorizationBehaviour<CommandTestPermission, ResponseCommand>(executionContextMock.Object);

        //Act
        var response = await behavior.Handle(request, nextMock.Object, cancellationToken);

        //Assert
        nextMock.Verify(n => n(), Times.Once);
    }

    [Fact]
    public async Task Handle_WithAuthorizationAttributeAndNoRoles_ShouldReturnForbidden()
    {
        //Arrange
        var executionContextMock = new Mock<IExecutionContext>();
        executionContextMock.Setup(e => e.HasRole("other-role")).Returns(true);

        var authorizeAttribute = new AuthorizeAttribute();

        var request = new CommandTestPermission();
        request.SomeProperty = "SomeValue";

        var requestMock = new Mock<CommandTestPermission>();
        requestMock.SetupGet(r => r.SomeProperty).Returns(request.SomeProperty);

        var nextMock = new Mock<RequestHandlerDelegate<ResponseCommand>>();
        nextMock.Setup(m => m.Invoke()).ReturnsAsync(new ResponseCommand(ResponseStatusCommand.Forbidden));
        var cancellationToken = new CancellationToken();

        var behavior = new AuthorizationBehaviour<CommandTestPermission, ResponseCommand>(executionContextMock.Object);

        //Act
        var response = await behavior.Handle(request, nextMock.Object, cancellationToken);

        //Assert
        Assert.Equal(ResponseStatusCommand.Forbidden, response.Status);
        Assert.Single(response.Errors);
        Assert.Equal($"{(int)SystemErrorType.CommandError}", response.Errors.First().Key);
        Assert.Equal("Forbidden access", response.Errors.First().Value);
    }

    [Fact]
    public async Task Handle_WithAuthorizationAttributeAndMatchingRoleAndPolicy_ShouldCallNext()
    {
        //Arrange
        var executionContextMock = new Mock<IExecutionContext>();
        executionContextMock.Setup(e => e.HasRole("role")).Returns(true);
        executionContextMock.Setup(e => e.HasPolicy("policy")).Returns(true);

        var authorizeAttribute = new AuthorizeAttribute();

        var request = new CommandTestPermission();
        request.SomeProperty = "SomeValue";

        var requestMock = new Mock<CommandTestPermission>();
        requestMock.SetupGet(r => r.SomeProperty).Returns(request.SomeProperty);

        var nextMock = new Mock<RequestHandlerDelegate<ResponseCommand>>();
        nextMock.Setup(m => m.Invoke()).ReturnsAsync(new ResponseCommand(ResponseStatusCommand.Forbidden));
        var cancellationToken = new CancellationToken();

        var behavior = new AuthorizationBehaviour<CommandTestPermission, ResponseCommand>(executionContextMock.Object);

        //Act
        var response = await behavior.Handle(request, nextMock.Object, cancellationToken);

        //Assert
        nextMock.Verify(n => n(), Times.Once);
    }

    [Fact]
    public async Task Handle_WithAuthorizationAttributeAndMatchingRoleNotPolicy_ShouldReturnForbidden()
    {
        //Arrange
        var executionContextMock = new Mock<IExecutionContext>();
        executionContextMock.Setup(e => e.HasRole("role")).Returns(true);
        executionContextMock.Setup(e => e.HasPolicy("policy")).Returns(false);

        var authorizeAttribute = new AuthorizeAttribute();

        var request = new CommandTestPermission();
        request.SomeProperty = "SomeValue";

        var requestMock = new Mock<CommandTestPermission>();
        requestMock.SetupGet(r => r.SomeProperty).Returns(request.SomeProperty);

        var nextMock = new Mock<RequestHandlerDelegate<ResponseCommand>>();
        nextMock.Setup(m => m.Invoke()).ReturnsAsync(new ResponseCommand(ResponseStatusCommand.Forbidden));
        var cancellationToken = new CancellationToken();

        var behavior = new AuthorizationBehaviour<CommandTestPermission, ResponseCommand>(executionContextMock.Object);

        //Act
        var response = await behavior.Handle(request, nextMock.Object, cancellationToken);

        //Assert
        Assert.Equal(ResponseStatusCommand.Forbidden, response.Status);
        Assert.Single(response.Errors);
        Assert.Equal($"{(int)SystemErrorType.CommandError}", response.Errors.First().Key);
        Assert.Equal("Forbidden access", response.Errors.First().Value);
    }
}