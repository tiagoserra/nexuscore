using Domain.Core.Entities;

namespace UnitTests.Domain.Core.Entities;

public class SystemGlobalizationTests
{
    [Fact]
    public void Create_SystemGlobalization()
    {
        // Act
        var systemglobalization = new SystemGlobalization("Common:Message:Required", new Dictionary<string, string>() { { "pt-BR", "Campo é obrigatório." }, { "en-US", "Field is mandatory." } });

        // Assert
        Assert.Equal("Common:Message:Required".ToLower(), systemglobalization.Key);
    }
}