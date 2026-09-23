namespace WorkTrack.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; protected set; }

    public DateTime CreatedAt { get; private set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }
}