using WorkTrack.Domain.Entities;
using WorkTrack.Domain.Enums;

namespace WorkTrack.Domain.Contracts;

public interface IWorkItemStore
{
    Task AddAsync(WorkItem item);
    Task<WorkItem?> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<WorkItem>> GetAllAsync(
    Priority? priority = null);
    Task<WorkItem?> GetForUpdateAsync(Guid id);
    Task SaveChangesAsync();
    void Remove(WorkItem item);
}