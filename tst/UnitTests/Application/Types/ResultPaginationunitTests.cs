using Application.Common.Types;

namespace UnitTests.Application.Types;

public class ResultPaginationunitTests
{
    [Fact]
    public void HasPrevious_ReturnsTrue_WhenPageIndexGreaterThanZero()
    {
        // Arrange
        var resultPagination = new ResultPagination(5, 1, 10, 50, null);

        // Act
        bool hasPrevious = resultPagination.HasPrevious;

        // Assert
        Assert.True(hasPrevious);
    }

    [Fact]
    public void HasPrevious_ReturnsFalse_WhenPageIndexIsZero()
    {
        // Arrange
        var resultPagination = new ResultPagination(5, 0, 10, 50, null);

        // Act
        bool hasPrevious = resultPagination.HasPrevious;

        // Assert
        Assert.False(hasPrevious);
    }

    [Fact]
    public void HasNext_ReturnsTrue_WhenPageIndexLessThanTotalPages()
    {
        // Arrange
        var resultPagination = new ResultPagination(5, 2, 10, 50, null);

        // Act
        bool hasNext = resultPagination.HasNext;

        // Assert
        Assert.True(hasNext);
    }

    [Fact]
    public void HasNext_ReturnsFalse_WhenPageIndexEqualToTotalPages()
    {
        // Arrange
        var resultPagination = new ResultPagination(5, 5, 10, 50, null);

        // Act
        bool hasNext = resultPagination.HasNext;

        // Assert
        Assert.False(hasNext);
    }
}