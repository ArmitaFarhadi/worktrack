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
    public DateTime DueDate { get; private set; }
    public WorkItem(
    string title,
    string description,
    Priority priority,
    DateTime dueDate )
{
    if (string.IsNullOrWhiteSpace(title))
    {
        throw new ArgumentException(
            "Title is required.",
            nameof(title));
    }

    if (!Enum.IsDefined(priority))
    {
        throw new ArgumentOutOfRangeException(
            nameof(priority),
            priority,
            "Priority must be a defined value.");
    }

    if (dueDate < DateTime.UtcNow)
    {
        throw new ArgumentException(
            "Due date cannot be in the past.",
            nameof(dueDate));
    }
    Title = title;
    Description = description;
    Priority = priority;
    Status = WorkItemStatus.Open;
    DueDate = dueDate;
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
        if (!Enum.IsDefined(priority))
        {
            throw new ArgumentOutOfRangeException(
                nameof(priority),
                priority,
                "Priority must be a defined value.");
        }

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