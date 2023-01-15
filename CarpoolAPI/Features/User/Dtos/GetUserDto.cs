namespace CarpoolAPI.Features.User.Dtos;

public class GetUserDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; } = string.Empty;
    public Driver.Driver? Driver { get; set; }
    public List<Ride.Ride> Rides { get; set; }
}
