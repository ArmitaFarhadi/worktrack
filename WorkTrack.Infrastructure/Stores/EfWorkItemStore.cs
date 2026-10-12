
using Microsoft.EntityFrameworkCore;
using WorkTrack.Domain.Contracts;
using WorkTrack.Domain.Entities;
using WorkTrack.Infrastructure.Data;

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

    public async Task<IReadOnlyCollection<WorkItem>> GetAllAsync()
    {
        return await _context.WorkItems
            .AsNoTracking()
            .Include(x => x.AssignedTo)
            .ToListAsync();
    }
}
