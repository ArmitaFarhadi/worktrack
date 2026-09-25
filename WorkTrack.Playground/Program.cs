using WorkTrack.Domain.Entities;
using WorkTrack.Domain.Enums;
using WorkTrack.Playground;

User user = new User(
    "Armita",
    "armita@example.com"
);

WorkItem loginWorkItem = new WorkItem(
    "Build login page",
    "Implement the first version of login",
    Priority.High
);

WorkItem footerWorkItem = new WorkItem(
    "Fix footer",
    "Correct footer spacing",
    Priority.Low
);

WorkItem dashboardWorkItem = new WorkItem(
    "Create dashboard",
    "Build dashboard layout",
    Priority.High
);

var workItems = new List<WorkItem>
{
    loginWorkItem,
    footerWorkItem,
    dashboardWorkItem
};


// ------------------------------------------------------------
// Day 2 - Domain behaviour
// ------------------------------------------------------------

Console.WriteLine("=== Domain behaviour ===");

Console.WriteLine($"Initial status: {loginWorkItem.Status}");
Console.WriteLine($"Assigned: {loginWorkItem.AssignedTo != null}");

loginWorkItem.AssignTo(user);

Console.WriteLine($"After assign: {loginWorkItem.Status}");
Console.WriteLine($"Assigned to: {loginWorkItem.AssignedTo?.Name}");

loginWorkItem.Close();

Console.WriteLine($"After close: {loginWorkItem.Status}");


// ------------------------------------------------------------
// Day 3 - Where
// ------------------------------------------------------------

Console.WriteLine();
Console.WriteLine("=== High-priority items ===");

var highPriorityItems =
    WorkItemQueries.GetHighPriorityItems(workItems);

foreach (var item in highPriorityItems)
{
    Console.WriteLine(item.Title);
}


// ------------------------------------------------------------
// Day 3 - Select
// ------------------------------------------------------------

Console.WriteLine();
Console.WriteLine("=== Titles ===");

var titles =
    WorkItemQueries.GetTitles(workItems);

foreach (var title in titles)
{
    Console.WriteLine(title);
}


// ------------------------------------------------------------
// Day 3 - OrderBy
// ------------------------------------------------------------

Console.WriteLine();
Console.WriteLine("=== Ordered by title ===");

var orderedItems =
    WorkItemQueries.GetItemsOrderedByTitle(workItems);

foreach (var item in orderedItems)
{
    Console.WriteLine(item.Title);
}


// ------------------------------------------------------------
// Day 3 - Any
// ------------------------------------------------------------

Console.WriteLine();
Console.WriteLine("=== Any ===");

bool hasHighPriority =
    WorkItemQueries.HasHighPriorityItems(workItems);

Console.WriteLine(
    $"Has high-priority items: {hasHighPriority}");


// ------------------------------------------------------------
// Day 3 - FirstOrDefault
// ------------------------------------------------------------

Console.WriteLine();
Console.WriteLine("=== Find by title ===");

WorkItem? foundItem =
    WorkItemQueries.FindByTitle(
        workItems,
        "Fix footer");

if (foundItem is not null)
{
    Console.WriteLine(
        $"Found: {foundItem.Title}");
}
else
{
    Console.WriteLine("Work item not found.");
}

WorkItem? missingItem =
    WorkItemQueries.FindByTitle(
        workItems,
        "Deploy API");

Console.WriteLine(
    missingItem is null
        ? "Deploy API was not found."
        : $"Found: {missingItem.Title}");


// ------------------------------------------------------------
// Day 3 - GroupBy
// ------------------------------------------------------------

Console.WriteLine();
Console.WriteLine("=== Grouped by priority ===");

var groups =
    WorkItemQueries.GroupByPriority(workItems);

foreach (var group in groups)
{
    Console.WriteLine($"Priority: {group.Key}");

    foreach (var item in group)
    {
        Console.WriteLine($"  {item.Title}");
    }
}


// ------------------------------------------------------------
// Day 3 - Dictionary
// ------------------------------------------------------------

Console.WriteLine();
Console.WriteLine("=== Dictionary by ID ===");

Dictionary<Guid, WorkItem> workItemsById =
    WorkItemQueries.IndexById(workItems);

if (workItemsById.TryGetValue(
        loginWorkItem.Id,
        out WorkItem? itemById))
{
    Console.WriteLine(
        $"Found by ID: {itemById.Title}");
}


// ------------------------------------------------------------
// Day 3 - HashSet
// ------------------------------------------------------------

Console.WriteLine();
Console.WriteLine("=== Unique titles ===");

HashSet<string> uniqueTitles =
    WorkItemQueries.GetUniqueTitles(workItems);

foreach (var title in uniqueTitles)
{
    Console.WriteLine(title);
}