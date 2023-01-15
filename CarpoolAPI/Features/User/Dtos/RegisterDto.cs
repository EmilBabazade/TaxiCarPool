using System.ComponentModel.DataAnnotations;

namespace CarpoolAPI.Features.User.Dtos;

public class RegisterDto
{
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Username { get; set; }
    public bool IsDriver { get; set; } = false;
    public string? carModel { get; set; }
    public int? seatCount { get; set; }
}
