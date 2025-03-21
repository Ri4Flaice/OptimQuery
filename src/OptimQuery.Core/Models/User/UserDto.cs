namespace OptimQuery.Core.Models.User;

public class UserDto
{
    public long Id { get; init; }
    public required string PhoneNumber { get; set; }
    public int Age { get; set; }
}