using WorkTrack.Domain.Contracts;
using WorkTrack.Domain.Entities;
using WorkTrack.Domain.Enums;

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

    public Task<IReadOnlyCollection<WorkItem>> GetAllAsync(
    Priority? priority = null)
{
    IEnumerable<WorkItem> query = _items;

    if (priority.HasValue)
    {
        query = query.Where(
            x => x.Priority == priority.Value);
    }

    IReadOnlyCollection<WorkItem> result = query
        .OrderByDescending(x => x.CreatedAt)
        .ToList();

    return Task.FromResult(result);
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