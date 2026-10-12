
using Microsoft.EntityFrameworkCore;
using WorkTrack.Domain.Entities;

namespace WorkTrack.Infrastructure.Data;

public class WorkTrackDbContext : DbContext
{
    public WorkTrackDbContext(
        DbContextOptions<WorkTrackDbContext> options)
        : base(options)
    {
    }

    public DbSet<WorkItem> WorkItems { get; set; } = null!;
}
