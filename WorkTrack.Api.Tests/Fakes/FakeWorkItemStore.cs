using WorkTrack.Domain.Contracts;
using WorkTrack.Domain.Dtos;
using WorkTrack.Domain.Entities;
using WorkTrack.Domain.Enums;

namespace WorkTrack.Api.Tests.Fakes;

public class FakeWorkItemStore : IWorkItemStore
{
    public Task<PagedResult<WorkItem>> GetAllAsync(
        Priority? priority = null,
        WorkItemStatus? status = null,
        string? sort = null,
        int? page = null,
        int? pageSize = null)
    {

        return Task.FromResult(new PagedResult<WorkItem>(
            Items: Items,
            TotalCount: Items.Count));
    }

    public IReadOnlyCollection<WorkItem> Items { get; set; }
    = Array.Empty<WorkItem>();
    public Task AddAsync(WorkItem item)
        => throw new NotSupportedException();

    public Task<WorkItem?> GetByIdAsync(Guid id)
        => throw new NotSupportedException();

    public Task<WorkItem?> GetForUpdateAsync(Guid id)
        => throw new NotSupportedException();

    public Task SaveChangesAsync()
        => throw new NotSupportedException();

    public void Remove(WorkItem item)
        => throw new NotSupportedException();
}