using Microsoft.Extensions.Localization;
using Application.Core.CommandHandlers;
using Application.Core.Commands;
using Application.Core.Interfaces;
using Domain.Core.Entities;
using Moq;

namespace UnitTests.Application.Core.Commands;

public class SystemGlobalizationRegisterCommandHandlerUnitTests
{
    private readonly Mock<IStringLocalizer> _mockLocalizer;
    private readonly Mock<ISystemGlobalizationRepository> _mockRepository;

    public SystemGlobalizationRegisterCommandHandlerUnitTests()
    {
        _mockLocalizer = new Mock<IStringLocalizer>();
        _mockRepository = new Mock<ISystemGlobalizationRepository>();
    }

    [Fact]
    public async Task Handle_ShouldRegisterSystemGlobalizationAndReturnResponseCommand()
    {
        // Arrange
        var id = 1;
        var systemglobalizationRegisterCommand = new SystemGlobalizationRegisterCommand("Common:Message:Required", new Dictionary<string, string>() { { "pt-BR", "Campo é obrigatório." }, { "en-US", "Field is mandatory." } });

        _mockRepository.Setup(repo => repo.InsertAsync(It.IsAny<SystemGlobalization>()))
            .Callback<SystemGlobalization>(systemglobalization => systemglobalization.Id = id)
            .Returns(Task.CompletedTask);

        var handler = new SystemGlobalizationRegisterCommandHandler(_mockLocalizer.Object, _mockRepository.Object);

        // Act
        var result = await handler.Handle(systemglobalizationRegisterCommand, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.ConvertTo<long>());
        _mockRepository.Verify(repo => repo.InsertAsync(It.IsAny<SystemGlobalization>()), Times.Once);
    }
}