using WorkTrack.Domain.Enums;

namespace WorkTrack.Api.Dtos;

public sealed record UpdateWorkItemPriorityRequest(
    Priority? Priority
);