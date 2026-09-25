using WorkTrack.Domain.Entities;
using WorkTrack.Domain.Enums;

namespace WorkTrack.Playground;

public static class WorkItemQueries
{
    public static IEnumerable<WorkItem> GetHighPriorityItems(
        IEnumerable<WorkItem> workItems)
    {
        return workItems.Where(
            item => item.Priority == Priority.High);
    }

    public static IEnumerable<string> GetTitles(
        IEnumerable<WorkItem> workItems)
    {
        return workItems.Select(
            item => item.Title);
    }

    public static IEnumerable<WorkItem> GetItemsOrderedByTitle(
        IEnumerable<WorkItem> workItems)
    {
        return workItems.OrderBy(
            item => item.Title);
    }

    public static bool HasHighPriorityItems(
        IEnumerable<WorkItem> workItems)
    {
        return workItems.Any(
            item => item.Priority == Priority.High);
    }

    public static WorkItem? FindByTitle(
        IEnumerable<WorkItem> workItems,
        string title)
    {
        return workItems.FirstOrDefault(
            item => item.Title == title);
    }

    public static IEnumerable<IGrouping<Priority, WorkItem>> GroupByPriority(
        IEnumerable<WorkItem> workItems)
    {
        return workItems.GroupBy(
            item => item.Priority);
    }

    public static Dictionary<Guid, WorkItem> IndexById(
        IEnumerable<WorkItem> workItems)
    {
        return workItems.ToDictionary(
            item => item.Id,
            item => item);
    }

    public static HashSet<string> GetUniqueTitles(
        IEnumerable<WorkItem> workItems)
    {
        return workItems
            .Select(item => item.Title)
            .ToHashSet();
    }
}