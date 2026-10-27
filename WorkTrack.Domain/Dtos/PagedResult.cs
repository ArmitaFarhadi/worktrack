namespace WorkTrack.Domain.Dtos;

public record PagedResult<T>(
    IReadOnlyCollection<T> Items,
    int TotalCount);