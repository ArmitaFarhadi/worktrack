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

