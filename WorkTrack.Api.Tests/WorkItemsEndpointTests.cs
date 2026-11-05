using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Text.Json;
using WorkTrack.Api.Tests.Helpers;
using WorkTrack.Domain.Entities;
using WorkTrack.Domain.Enums;

namespace WorkTrack.Api.Tests;

public class WorkItemsEndpointTests
{
    [Fact]
    public async Task GetOverdue_WhenNoItems_ReturnsEmptyArray()
    {
        // Arrange
        using var factory = new WorkTrackApiFactory();

        using var client = factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost")
            });

        // Act
        var response = await client.GetAsync(
            "/api/workitems/overdue");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal("[]", content.Trim());
    }

    [Fact]
public async Task GetOverdue_WhenOpenItemIsOverdue_ReturnsItem()
{
    // Arrange: simulate when the item was created
    var creationClock = new FixedTimeProvider(
        new DateTimeOffset(
            2026, 11, 1, 12, 0, 0,
            TimeSpan.Zero));

    var dueDate = new DateTime(
        2026, 11, 3, 12, 0, 0,
        DateTimeKind.Utc);

    var item = new WorkItem(
        "Test overdue item",
        "Testing overdue filtering",
        Priority.High,
        dueDate,
        creationClock);

    // Arrange: simulate the current API time
    var apiClock = new FixedTimeProvider(
        new DateTimeOffset(
            2026, 11, 5, 12, 0, 0,
            TimeSpan.Zero));

    using var factory = new WorkTrackApiFactory(
        new[] { item },
        apiClock);

    using var client = factory.CreateClient();

    // Act
    var response = await client.GetAsync(
        "/api/workitems/overdue");

    // Assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var json = await response.Content.ReadAsStringAsync();

    using var document = JsonDocument.Parse(json);

    Assert.Equal(
        JsonValueKind.Array,
        document.RootElement.ValueKind);

    // YOUR TASK: Assert that the array contains exactly one item.
   Assert.Single(document.RootElement.EnumerateArray());
}

[Fact]
public async Task GetOverdue_WhenDueDateIsFuture_ReturnsEmptyArray()
{
    // Arrange
    var creationClock = new FixedTimeProvider(
        new DateTimeOffset(
            2026, 11, 1, 12, 0, 0,
            TimeSpan.Zero));

    var apiClock = new FixedTimeProvider(
        new DateTimeOffset(
            2026, 11, 5, 12, 0, 0,
            TimeSpan.Zero));

    var dueDate = new DateTime(
        2026, 11, 8, 12, 0, 0,
        DateTimeKind.Utc);

    // TODO 1: Create a WorkItem using creationClock
    var item = new WorkItem(
        "Test future item",
        "Testing future due date filtering",
        Priority.Medium,
        dueDate,
        creationClock);

    using var factory = new WorkTrackApiFactory(
        new[] { item },
        apiClock);

    using var client = factory.CreateClient();

    var response = await client.GetAsync("/api/workitems/overdue");

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var json = await response.Content.ReadAsStringAsync();

    using var document = JsonDocument.Parse(json);

    Assert.Equal(
        JsonValueKind.Array,
        document.RootElement.ValueKind);

    Assert.Empty(document.RootElement.EnumerateArray());
}

[Fact]
public async Task GetOverdue_WhenItemIsClosed_ReturnsEmptyArray()
{
    // Arrange
    var creationClock = new FixedTimeProvider(
        new DateTimeOffset(
            2026, 11, 1, 12, 0, 0,
            TimeSpan.Zero));

    var apiClock = new FixedTimeProvider(
        new DateTimeOffset(
            2026, 11, 5, 12, 0, 0,
            TimeSpan.Zero));

    var dueDate = new DateTime(
        2026, 11, 3, 12, 0, 0,
        DateTimeKind.Utc);

    var item = new WorkItem(
        "Test closed item",
        "Testing closed item filtering",
        Priority.Low,
        dueDate,
        creationClock);

    item.Close();

    Assert.Equal(WorkItemStatus.Closed, item.Status);

    using var factory = new WorkTrackApiFactory(
        new[] { item },
        apiClock);

    using var client = factory.CreateClient();

    var response = await client.GetAsync("/api/workitems/overdue");

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var json = await response.Content.ReadAsStringAsync();

    using var document = JsonDocument.Parse(json);

    Assert.Equal(
        JsonValueKind.Array,
        document.RootElement.ValueKind);

    Assert.Empty(document.RootElement.EnumerateArray());
}
}