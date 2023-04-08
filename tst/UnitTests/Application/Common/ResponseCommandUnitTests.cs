using Application.Common.Commands;
using Application.Common.Enums;

namespace UnitTests.Application.Common;

public class ResponseCommandUnitTests
{
    [Fact]
    public void ResponseCommandShouldBeOKWithParameter()
    {
        var response = new ResponseCommand(new { a = 1});

        Assert.Equal(ResponseStatusCommand.Ok, response.Status);
    }

    [Fact]
    public void ResponseCommandShouldBeErrorWithParameter()
    {
        var response = new ResponseCommand(ResponseStatusCommand.Error);

        Assert.Equal(ResponseStatusCommand.Error, response.Status);
    }

    [Theory]
    [InlineData("001", "erro message")]
    public void ResponseCommandShouldBeErrorWhenCallAddError(string code, string message)
    {
        var response = new ResponseCommand(ResponseStatusCommand.Error);

        response.AddError(code, message);

        Assert.Equal(ResponseStatusCommand.Error, response.Status);
        Assert.Equal(message, response.Errors[code]);
    }

}
