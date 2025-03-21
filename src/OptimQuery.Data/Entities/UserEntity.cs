using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using OptimQuery.Core.Interfaces.User;

namespace OptimQuery.Data.Entities;

[Table("users")]
[Index(nameof(Age),
    nameof(IsBlocked),
    nameof(IsTwoFactorAuthEnabled),
    nameof(Id),
    Name = "UserIndex",
    IsUnique = false)]
public class UserEntity : IUser
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [MaxLength(50)]
    public required string Username { get; set; }
    
    [MaxLength(256)]
    public required string Email { get; set; }

    [MaxLength(12)]
    public required string PhoneNumber { get; set; }

    public int Age { get; set; }
    public DateTime Birthday { get; set; }
    public DateTime RegistrationDate { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsBlocked { get; set; }
    public bool IsTwoFactorAuthEnabled { get; set; }
}