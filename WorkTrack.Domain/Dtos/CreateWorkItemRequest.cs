using WorkTrack.Domain.Enums;

namespace WorkTrack.Domain.Dtos;

public record CreateWorkItemRequest(
    string Title,
    string Description,
    Priority Priority);