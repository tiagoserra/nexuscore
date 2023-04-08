using Application.Common.Behaviors;
using Application.Common.Commands;
using Application.Common.Enums;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;

namespace UnitTests.Application.Behaviors;

public class ValidationBehaviourUnitTests
{
    [Fact]
    public async Task Handle_WithValidationFailures_ShouldReturnErrorResponse()
    {
        // Arrange
        var request = new SampleRequest();
        var validatorMock = new Mock<IValidator<SampleRequest>>();
        validatorMock
            .Setup(v => v.Validate(request))
            .Returns(new ValidationResult(new List<ValidationFailure>
            {
            new ValidationFailure("TestProperty", "Test error message")
            }));

        var validators = new List<IValidator<SampleRequest>> { validatorMock.Object };
        var requestHandlerDelegateMock = new Mock<RequestHandlerDelegate<ResponseCommand>>();

        var validationBehaviour = new ValidationBehaviour<SampleRequest, ResponseCommand>(validators);

        // Act
        var response = await validationBehaviour.Handle(request, requestHandlerDelegateMock.Object, CancellationToken.None);

        // Assert
        Assert.Equal(ResponseStatusCommand.Error, response.Status);
        Assert.Single(response.Errors);
        Assert.True(response.Errors.Keys.Any(key => key.Contains("TestProperty")), "TestProperty not found in error keys.");
        Assert.Contains("Test error message", response.Errors.Values.First());
    }

    [Fact]
    public async Task Handle_WithNoValidationFailures_ShouldCallNextDelegate()
    {
        // Arrange
        var request = new SampleRequest();
        var validatorMock = new Mock<IValidator<SampleRequest>>();
        validatorMock
            .Setup(v => v.Validate(request))
            .Returns(new ValidationResult());

        var validators = new List<IValidator<SampleRequest>> { validatorMock.Object };
        var requestHandlerDelegateMock = new Mock<RequestHandlerDelegate<ResponseCommand>>();
        requestHandlerDelegateMock
            .Setup(x => x())
            .ReturnsAsync(new ResponseCommand(new object()));

        var validationBehaviour = new ValidationBehaviour<SampleRequest, ResponseCommand>(validators);

        // Act
        var response = await validationBehaviour.Handle(request, requestHandlerDelegateMock.Object, CancellationToken.None);

        // Assert
        requestHandlerDelegateMock.Verify(x => x(), Times.Once());
        Assert.Equal(ResponseStatusCommand.Ok, response.Status);
    }
}