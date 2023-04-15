using Application.Common.Enums;
using Application.Core.CommandHandlers;
using Application.Core.Commands;
using Application.Core.Interfaces;
using Domain.Core.Entities;
using Microsoft.Extensions.Localization;
using Moq;

namespace UnitTests.Application.Core.Commands;

public class SystemGlobalizationAlterCommandUnitTests
{
    [Fact]
    public async Task Handle_SystemGlobalizationExists_ShouldAlterNameAndReturnUpdatedSystemGlobalization()
    {
        // Arrange
        string key = "Common:Message:Required";
        var newValue = new Dictionary<string, string>() { { "pt-BR", "Campo é obrigatório" }, { "en-US", "Field is mandatory" } };

        var systemglobalization = new SystemGlobalization("Common:Message:Required", new Dictionary<string, string>() { { "pt-BR", "Campo é obrigatório" }, { "en-US", "Field is mandatory" } });
        var command = new SystemGlobalizationAlterCommand(key, newValue);

        var localizerMock = new Mock<IStringLocalizer>();
        var repositoryMock = new Mock<ISystemGlobalizationRepository>();

        repositoryMock.Setup(r => r.GetByKeyAsync(key)).ReturnsAsync(systemglobalization);
        repositoryMock.Setup(r => r.UpdateAsync(systemglobalization)).Returns(Task.CompletedTask);

        var handler = new SystemGlobalizationAlterCommandHandler(localizerMock.Object, repositoryMock.Object);

        // Act
        var response = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(ResponseStatusCommand.Ok, response.Status);
        Assert.Equal(newValue, ((SystemGlobalization)response.Result).Resource);

        repositoryMock.Verify(r => r.GetByKeyAsync(key), Times.Once);
        repositoryMock.Verify(r => r.UpdateAsync(systemglobalization), Times.Once);
    }

    [Fact]
    public async Task Handle_SystemGlobalizationDoesNotExist_ShouldReturnNotFoundResponse()
    {
        // Arrange
        string key = "Common:Message:Required";
        var newValue = new Dictionary<string, string>() { { "pt-BR", "Campo é obrigatório" }, { "en-US", "Field is mandatory" } };

        var command = new SystemGlobalizationAlterCommand(key, newValue);

        var localizerMock = new Mock<IStringLocalizer>();
        var repositoryMock = new Mock<ISystemGlobalizationRepository>();

        repositoryMock.Setup(r => r.GetByKeyAsync(key)).ReturnsAsync((SystemGlobalization)null);

        var handler = new SystemGlobalizationAlterCommandHandler(localizerMock.Object, repositoryMock.Object);

        // Act
        var response = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(ResponseStatusCommand.NotFound, response.Status);

        repositoryMock.Verify(r => r.GetByKeyAsync(key), Times.Once);
        repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<SystemGlobalization>()), Times.Never);
    }
}