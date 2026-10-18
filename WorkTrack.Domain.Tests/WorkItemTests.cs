using WorkTrack.Domain.Entities;
using WorkTrack.Domain.Enums;
using Xunit;

namespace WorkTrack.Domain.Tests;

public class WorkItemTests
{
    [Fact]
    public void Close_WhenWorkItemIsOpen_SetsClosedStatus()
    {
        // Arrange
        var item = new WorkItem(
            "Learn automated testing",
            "Day 10 practice",
            Priority.High);

        // Act
        item.Close();

        // Assert
        Assert.Equal(WorkItemStatus.Closed, item.Status);
        Assert.NotNull(item.ClosedAt);

    }
    [Fact]
public void Close_WhenAlreadyClosed_ThrowsInvalidOperationException()
{
    // Arrange
    var item = new WorkItem(
        "Learn unit testing",
        "Day 10 practice",
        Priority.High);

    item.Close();

    // Act and Assert
    Assert.Throws<InvalidOperationException>(
        () => item.Close());
}

[Theory]
[InlineData(999)]
[InlineData(-1)]
[InlineData(1000)]
public void Constructor_WithInvalidPriority_ThrowsException(
    int invalidPriority)
{
    // Act and Assert
    Assert.Throws<ArgumentOutOfRangeException>(
        () => new WorkItem(
            "Invalid priority test",
            "Day 10 practice",
            (Priority)invalidPriority));
}
}
