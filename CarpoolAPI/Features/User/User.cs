using System.ComponentModel.DataAnnotations;

namespace CarpoolAPI.Features.User;

public class User
{
    [Key]
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public int? DriverId { get; set; }
    public Driver.Driver? Driver { get; set; }
    public List<Ride.Ride> Rides { get; set; }
}
