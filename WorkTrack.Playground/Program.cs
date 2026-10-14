using WorkTrack.Domain.Dtos;
using WorkTrack.Domain.Entities;
using WorkTrack.Domain.Enums;
using WorkTrack.Domain.Contracts;
using WorkTrack.Domain.Stores;

var store = new InMemoryWorkItemStore();

var request = new CreateWorkItemRequest(
    "Fix login bug",
    "Build the initial dashboard page.",
    Priority.Medium);

var first = new WorkItem(
    request.Title,
    request.Description,
    request.Priority);
    
var second = new WorkItem(
    "Build dashboard",
    "Set up user authentication and authorization.",
    Priority.High);

await store.AddAsync(first);
await store.AddAsync(second);

IReadOnlyCollection<WorkItem> allItems =
    await store.GetAllAsync();

Console.WriteLine($"Count: {allItems.Count}");

WorkItem? found =
    await store.GetByIdAsync(first.Id);

Console.WriteLine(found?.Title ?? "Not found");

WorkItem? missing =
    await store.GetByIdAsync(Guid.NewGuid());

Console.WriteLine(missing?.Title ?? "Not found");


// DAY 9 - TEST 1: Invalid priority in constructor

bool constructorRejected = false;

try
{
    var invalidItem = new WorkItem(
        "Invalid WorkItem",
        "Testing invalid priority",
        (Priority)999
    );
}
catch (ArgumentOutOfRangeException)
{
    constructorRejected = true;
}

Console.WriteLine(
    $"Constructor rejected invalid priority: {constructorRejected}");

if (!constructorRejected)
{
    throw new Exception(
        "TEST FAILED: Constructor accepted invalid priority.");
}

// DAY 9 - TEST 2: Invalid priority change

Priority originalPriority = first.Priority;

bool changeRejected = false;

try
{
    first.ChangePriority((Priority)999);
}
catch (ArgumentOutOfRangeException)
{
    changeRejected = true;
}

Console.WriteLine(
    $"ChangePriority rejected invalid priority: {changeRejected}");

Console.WriteLine(
    $"Original priority preserved: {first.Priority == originalPriority}");

if (!changeRejected || first.Priority != originalPriority)
{
    throw new Exception(
        "TEST FAILED: Invalid priority change was not handled correctly.");
}