namespace WorkTrack.Api.Dtos;

public sealed record PagedWorkItemsResponse(
    int? Page,
    int? PageSize,
    int TotalCount,
    List<WorkItemResponse> Items);