
using Microsoft.EntityFrameworkCore;
using WorkTrack.Domain.Contracts;
using WorkTrack.Domain.Entities;
using WorkTrack.Infrastructure.Data;
using WorkTrack.Domain.Enums;

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

    public async Task<IReadOnlyCollection<WorkItem>> GetAllAsync(
    Priority? priority = null)
{
    IQueryable<WorkItem> query = _context.WorkItems
        .AsNoTracking()
        .Include(x => x.AssignedTo);

    if (priority.HasValue)
    {
        query = query.Where(
            x => x.Priority == priority.Value);
    }

    return await query
        .OrderByDescending(x => x.CreatedAt)
        .ToListAsync();
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
