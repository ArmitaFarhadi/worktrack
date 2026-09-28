using WorkTrack.Domain.Contracts;
using WorkTrack.Domain.Entities;

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

    public Task<IReadOnlyCollection<WorkItem>> GetAllAsync()
    {
        IReadOnlyCollection<WorkItem> result =
            _items.ToList();

        return Task.FromResult(result);
    }
}