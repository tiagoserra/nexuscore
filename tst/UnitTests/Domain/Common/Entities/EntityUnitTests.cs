using Domain.Common.Entities;

namespace UnitTests.Domain.Common.Entities;

public class EntityUnitTests
{
    [Fact]
    public void SetCreation_ShouldSetCreatedOnAndCreatedBy()
    {
        //Arrange
        Entity entity = new TestEntity();
        var createdBy = "John Doe";

        //Act
        entity.SetCreation(createdBy);

        //Assert
        Assert.Equal(createdBy, entity.CreatedBy);
    }

    [Fact]
    public void SetModification_ShouldSetModifiedOnAndModifiedBy()
    {
        //Arrange
        Entity entity = new TestEntity();
        var modifiedBy = "Jane Doe";

        //Act
        entity.SetModification(modifiedBy);

        //Assert
        Assert.Equal(modifiedBy, entity.ModifiedBy);
        Assert.NotNull(entity.ModifiedOn);
    }
}

public class TestEntity : Entity
{
    //Empty class used only for testing purposes
}