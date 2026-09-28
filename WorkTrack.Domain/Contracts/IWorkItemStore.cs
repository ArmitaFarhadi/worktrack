using WorkTrack.Domain.Entities;

namespace WorkTrack.Domain.Contracts;

public interface IWorkItemStore
{
    Task AddAsync(WorkItem item);

    Task<WorkItem?> GetByIdAsync(Guid id);

    Task<IReadOnlyCollection<WorkItem>> GetAllAsync();
}