namespace CarpoolAPI.Features.Dto;

public class RideDto
{
    public int DriverID { get; set; }
    public string DriverUsername { get; set; }
    public string SourceLocation { get; set; }
    public string DestinationLocation { get; set; }
}
