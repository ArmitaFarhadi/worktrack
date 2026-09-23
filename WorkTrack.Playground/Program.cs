using WorkTrack.Domain.Entities;
using WorkTrack.Domain.Enums;

User user = new User(
    "Armita",
    "armita@example.com"
);

WorkItem item = new WorkItem(
    "Build login page",
    "Implement the first version of login",
    Priority.High
);

Console.WriteLine($"Initial status: {item.Status}");
Console.WriteLine($"Assigned: {item.AssignedTo != null}");

item.AssignTo(user);

Console.WriteLine($"After assign: {item.Status}");
Console.WriteLine($"Assigned to: {item.AssignedTo?.Name}");

item.Close();

Console.WriteLine($"After close: {item.Status}");