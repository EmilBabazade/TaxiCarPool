namespace CarpoolAPI.Features.Ride;

public class Ride
{
    public int Id { get; set; }
    public DayOfWeek day { get; set; }
    public string startLocation { get; set; } = string.Empty;
    public string endLocation { get; set; } = string.Empty;
    public int DriverId { get; set; }
    public Driver.Driver Driver { get; set; }
    public List<User.User> Users { get; set; }
}
