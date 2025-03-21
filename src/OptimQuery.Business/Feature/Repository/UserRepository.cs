using EFCore.BulkExtensions;

using Microsoft.EntityFrameworkCore;

using OptimQuery.Business.Feature.Common.Cursor;
using OptimQuery.Core.Interfaces.Repository;
using OptimQuery.Core.Models;
using OptimQuery.Core.Models.User;
using OptimQuery.Data.Data;
using OptimQuery.Data.Entities;

namespace OptimQuery.Business.Feature.Repository;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    private readonly Random _random = new();

    public async Task AddUsersAsync(CancellationToken cancellationToken)
    {
        try
        {
            const int totalUsers = 10_000_000;
            var users = new List<UserEntity>(totalUsers);

            for (var i = 0; i < totalUsers; i++)
            {
                var user = GenerateRandomUser(i);
                users.Add(user);
            }

            await dbContext.BulkInsertAsync(users, cancellationToken: cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<PaginatedResult<UserDto>> GetUsersAsync(
        string? cursor, int limit, CancellationToken cancellationToken)
    {
        try
        {
            var query = dbContext.UserEntities
                .AsNoTracking()
                .AsQueryable();

            query = query.Where(user => user.Age == 18 &&
                                        !user.IsBlocked &&
                                        user.IsTwoFactorAuthEnabled);

            if (!string.IsNullOrWhiteSpace(cursor))
            {
                var decodedCursor = CursorDb.Decode(cursor);

                if (decodedCursor is null) throw new InvalidOperationException("Invalid cursor");
                
                query = query.Where(user => user.Id > decodedCursor.LastId);
            }

            var items = await query
                .OrderBy(user => user.Id) 
                .Take(limit + 1)
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    PhoneNumber = user.PhoneNumber,
                    Age = user.Age,
                })
                .ToListAsync(cancellationToken);

            var hasMore = items.Count > limit;
            long? nextId = items.Count > limit ? items[^1].Id : null;

            items.RemoveAt(items.Count - 1);

            return new PaginatedResult<UserDto>
            {
                Items = items,
                Cursor = nextId.HasValue ? CursorDb.Encode(nextId.Value) : null,
                HasMore = hasMore,
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await dbContext.UserEntities.CountAsync(cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private UserEntity GenerateRandomUser(int index)
    {
        var age = _random.Next(18, 81);
        var birthday = DateTime.UtcNow.AddYears(-age).AddDays(_random.Next(-365, 365));
        var registrationDate = DateTime.UtcNow.AddDays(-_random.Next(0, 3650));

        return new UserEntity
        {
            Username = $"User_{index}_{GenerateRandomString(8)}",
            Email = $"user_{index}_{GenerateRandomString(6)}@example.com",
            PhoneNumber = GenerateRandomPhoneNumber(),
            Age = age,
            Birthday = birthday,
            RegistrationDate = registrationDate,
            IsAdmin = _random.NextDouble() < 0.05,
            IsBlocked = _random.NextDouble() < 0.1,
            IsTwoFactorAuthEnabled = _random.NextDouble() < 0.3
        };
    }

    private string GenerateRandomString(int length)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }

    private string GenerateRandomPhoneNumber()
    {
        return $"+7{_random.Next(900, 999)}{_random.Next(100, 999)}{_random.Next(10, 99)}{_random.Next(10, 99)}";
    }
}