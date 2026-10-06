using WorkTrack.Domain.Enums;

namespace WorkTrack.Api.Dtos;

public record WorkItemResponse(
    Guid Id,
    string Title,
    string Description,
    WorkItemStatus Status,
    Priority Priority,
    Guid? AssignedToId,
    DateTime CreatedAt,
    DateTime? ClosedAt);