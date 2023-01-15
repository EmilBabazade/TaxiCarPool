namespace CarpoolAPI.Features.Driver;

public class Driver
{
    public int Id { get; set; }
    public string carModel { get; set; } = string.Empty;
    public int seatCount { get; set; }
    public int UserId { get; set; }
    public User.User User { get; set; }
    public List<Ride.Ride> Rides { get; set; }
}
