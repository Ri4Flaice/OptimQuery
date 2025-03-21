using OptimQuery.Core.Models;
using OptimQuery.Core.Models.User;

namespace OptimQuery.Core.Interfaces.Repository;

public interface IUserRepository
{
    Task AddUsersAsync(CancellationToken cancellationToken);
    Task<PaginatedResult<UserDto>> GetUsersAsync(
        string? cursor, int limit, CancellationToken cancellationToken);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken);
}