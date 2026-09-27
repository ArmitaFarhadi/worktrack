using WorkTrack.Domain.Dtos;
using WorkTrack.Domain.Entities;
using WorkTrack.Domain.Enums;

var request = new CreateWorkItemRequest(
    "Create dashboard",
    "Build the initial dashboard page.",
    Priority.Medium);

var workItem = new WorkItem(
    request.Title,
    request.Description,
    request.Priority);

Console.WriteLine($"Title: {workItem.Title}");
Console.WriteLine($"Status: {workItem.Status}");
Console.WriteLine(
    $"Assigned to: {workItem.AssignedTo?.Name ?? "Unassigned"}");
Console.WriteLine(
    $"Closed at: {workItem.ClosedAt?.ToString("O") ?? "Not closed"}");

workItem.Close();

try
{
    workItem.ChangePriority(Priority.High);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Domain error: {ex.Message}");
}

Console.WriteLine($"Priority: {workItem.Priority}");