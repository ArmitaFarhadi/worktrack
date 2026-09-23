using WorkTrack.Domain.Contracts;
using WorkTrack.Domain.Enums;

namespace WorkTrack.Domain.Entities;

public class WorkItem : Entity, IClosable
{
    public string Title { get; private set; }

    public string Description { get; private set; }

    public WorkItemStatus Status { get; private set; }

    public Priority Priority { get; private set; }

    public User? AssignedTo { get; private set; }

    public DateTime? ClosedAt { get; private set; }

    public WorkItem(
        string title,
        string description,
        Priority priority)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException(
                "Title is required.",
                nameof(title));
        }

        Title = title;
        Description = description;
        Priority = priority;
        Status = WorkItemStatus.Open;
    }

    public void AssignTo(User user)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (Status == WorkItemStatus.Closed)
        {
            throw new InvalidOperationException(
                "A closed work item cannot be assigned.");
        }

        AssignedTo = user;
        Status = WorkItemStatus.InProgress;
    }

    public void ChangePriority(Priority priority)
    {
        if (Status == WorkItemStatus.Closed)
        {
            throw new InvalidOperationException(
                "Priority cannot be changed after the work item is closed.");
        }

        Priority = priority;
    }

    public void Close()
    {
        if (Status == WorkItemStatus.Closed)
        {
            throw new InvalidOperationException(
                "Work item is already closed.");
        }

        Status = WorkItemStatus.Closed;
        ClosedAt = DateTime.UtcNow;
    }
}