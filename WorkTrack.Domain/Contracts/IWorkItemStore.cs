using WorkTrack.Domain.Dtos;
using WorkTrack.Domain.Entities;
using WorkTrack.Domain.Enums;

namespace WorkTrack.Domain.Contracts;

public interface IWorkItemStore
{
    Task AddAsync(WorkItem item);

    Task<WorkItem?> GetByIdAsync(Guid id);

    Task<PagedResult<WorkItem>> GetAllAsync(
        Priority? priority = null,
        WorkItemStatus? status = null,
        string? sort = null,
        int? page = null,
        int? pageSize = null);

    Task<WorkItem?> GetForUpdateAsync(Guid id);

    Task SaveChangesAsync();

    void Remove(WorkItem item);
}