namespace OptimQuery.Core.Models;

public class PaginatedResult<T>
{
    public List<T> Items { get; init; } = [];
    public string? Cursor { get; init; }
    public bool HasMore { get; init; }
}