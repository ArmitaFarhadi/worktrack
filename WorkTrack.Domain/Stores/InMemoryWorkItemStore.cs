using WorkTrack.Domain.Contracts;
using WorkTrack.Domain.Entities;
using WorkTrack.Domain.Enums;
using WorkTrack.Domain.Dtos;

namespace WorkTrack.Domain.Stores;

public class InMemoryWorkItemStore : IWorkItemStore
{
    private readonly List<WorkItem> _items = new();

    public Task AddAsync(WorkItem item)
    {
        _items.Add(item);

        return Task.CompletedTask;
    }

    public Task<WorkItem?> GetByIdAsync(Guid id)
    {
        WorkItem? item =
            _items.FirstOrDefault(x => x.Id == id);

        return Task.FromResult(item);
    }

   public Task<PagedResult<WorkItem>> GetAllAsync(
    Priority? priority = null,
    WorkItemStatus? status = null,
    string? sort = null,
    int? page = null,
    int? pageSize = null)
{
    IEnumerable<WorkItem> query = _items;

    if (priority.HasValue)
    {
        query = query.Where(
            x => x.Priority == priority.Value);
    }

    if (status.HasValue)
    {
        query = query.Where(
            x => x.Status == status.Value);
    }

    var totalCount = query.Count();

    if (string.Equals(
        sort,
        "dueDate",
        StringComparison.OrdinalIgnoreCase))
    {
        query = query
            .OrderBy(x => x.DueDate)
            .ThenBy(x => x.Id);
    }
    else
    {
        query = query
            .OrderByDescending(x => x.CreatedAt)
            .ThenBy(x => x.Id);
    }

    if (page.HasValue && pageSize.HasValue)
    {
        query = query
            .Skip((page.Value - 1) * pageSize.Value)
            .Take(pageSize.Value);
    }

    IReadOnlyCollection<WorkItem> items =
        query.ToList();

    return Task.FromResult(
        new PagedResult<WorkItem>(
            items,
            totalCount));
}
    public Task<WorkItem?> GetForUpdateAsync(Guid id)
{
    return GetByIdAsync(id);
}

    public Task SaveChangesAsync()
    {
        // In-memory store does not require saving changes.
        return Task.CompletedTask;
    }

    public void Remove(WorkItem item)
{
    _items.Remove(item);
}
}