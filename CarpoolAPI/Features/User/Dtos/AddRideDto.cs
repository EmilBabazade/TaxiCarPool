using System.ComponentModel.DataAnnotations;

namespace CarpoolAPI.Features.User.Dtos;

public class AddRideDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string SourceLocation { get; set; }
    [Required]
    public string DestinationLocation { get; set; }
    [Required]
    public DayOfWeek day { get; set; }
}
