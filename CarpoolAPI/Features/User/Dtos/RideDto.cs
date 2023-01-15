namespace CarpoolAPI.Features.User.Dtos;

public class RideDto
{
    public int RideId { get; set; }
    public string Driverusername { get; set; }
    public DayOfWeek Day { get; set; }
    public string StartLocation { get; set; }
    public string EndLocation { get; set; }
}
