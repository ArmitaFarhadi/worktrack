
using Microsoft.EntityFrameworkCore;
using WorkTrack.Domain.Contracts;
using WorkTrack.Domain.Entities;
using WorkTrack.Infrastructure.Data;
using WorkTrack.Domain.Enums;
using WorkTrack.Domain.Dtos;

namespace WorkTrack.Infrastructure.Stores;

public class EfWorkItemStore : IWorkItemStore
{
    private readonly WorkTrackDbContext _context;

    public EfWorkItemStore(WorkTrackDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(WorkItem item)
    {
        _context.WorkItems.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task<WorkItem?> GetByIdAsync(Guid id)
    {
        return await _context.WorkItems
            .AsNoTracking()
            .Include(x => x.AssignedTo)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PagedResult<WorkItem>> GetAllAsync(
    Priority? priority = null,
    WorkItemStatus? status = null,
    string? sort = null,
    int? page = null,
    int? pageSize = null)
{
    IQueryable<WorkItem> query = _context.WorkItems
        .AsNoTracking()
        .Include(x => x.AssignedTo);

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

    var totalCount = await query.CountAsync();

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

    var items = await query.ToListAsync();

    return new PagedResult<WorkItem>(
        items,
        totalCount);
}
    public async Task<WorkItem?> GetForUpdateAsync(Guid id)
    {
        return await _context.WorkItems
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Remove(WorkItem item)
    {
        _context.WorkItems.Remove(item);
    }

}
