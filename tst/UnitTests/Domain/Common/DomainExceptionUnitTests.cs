using Domain.Common.Exceptions;

namespace UnitTests.Domain.Common;

public class DomainExceptionUnitTests
{
    [Fact]
    public void When_ConditionIsTrue_ShouldThrowDomainException()
    {
        //Arrange
        bool condition = true;
        string field = "FieldName";
        string messageError = "Message Error";

        //Act and Assert
        Assert.Throws<DomainException>(() => DomainException.When(condition, field, messageError));
    }

    [Fact]
    public void When_ConditionIsFalse_ShouldNotThrowDomainException()
    {
        //Arrange
        bool condition = false;
        string field = "FieldName";
        string messageError = "Message Error";

        //Act and Assert
        DomainException.When(condition, field, messageError);
        //No exception is thrown
    }
}
