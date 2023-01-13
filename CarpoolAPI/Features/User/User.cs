using System.ComponentModel.DataAnnotations;

namespace CarpoolAPI.Features.User;

public class User
{
    [Key]
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
}
