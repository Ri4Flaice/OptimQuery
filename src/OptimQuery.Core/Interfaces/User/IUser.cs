namespace OptimQuery.Core.Interfaces.User;

public interface IUser
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public int Age { get; set; }
    public DateTime Birthday { get; set; }
    public DateTime RegistrationDate { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsBlocked { get; set; }
    public bool IsTwoFactorAuthEnabled { get; set; }
}