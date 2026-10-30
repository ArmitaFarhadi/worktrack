
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WorkTrack.Domain.Entities;
using WorkTrack.Domain.Enums;
using WorkTrack.Infrastructure.Data;

namespace WorkTrack.Infrastructure.Tests;

public class WorkItemMaterializationTests
{
    [Fact]
    public async Task GetOverdueWorkItem_FromFreshContext_Succeeds()
    {
        // ARRANGE: Create a temporary SQLite database
        using var connection = new SqliteConnection(
            "Data Source=:memory:");

        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<WorkTrackDbContext>()
            .UseSqlite(connection)
            .Options;

        Guid workItemId;
        var overdueDate = DateTime.UtcNow.AddDays(-7);

        // ARRANGE: Save a valid new work item
        await using (var context = new WorkTrackDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();

            var item = new WorkItem(
                "Test overdue work item",
                "Testing EF Core materialization",
                Priority.High,
                DateTime.UtcNow.AddDays(7));

            context.WorkItems.Add(item);
            await context.SaveChangesAsync();

            workItemId = item.Id;
        }

        // ARRANGE: Make the saved item overdue
        await using (var context = new WorkTrackDbContext(options))
        {
            var updatedRows = await context.WorkItems
                .Where(w => w.Id == workItemId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(w => w.DueDate, overdueDate));

            Assert.Equal(1, updatedRows);
        }

        // ACT: Load the overdue item from a NEW context
        await using (var context = new WorkTrackDbContext(options))
        {
            var loadedItem = await context.WorkItems
                .AsNoTracking()
                .SingleAsync(w => w.Id == workItemId);

            // ASSERT: EF Core successfully reconstructed it
            Assert.Equal(workItemId, loadedItem.Id);
            Assert.Equal(overdueDate, loadedItem.DueDate);
        }
    }
}
