using CarpoolAPI.Features.User.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarpoolAPI.Features.Ride;
[Route("api/[controller]")]
[ApiController]
public class RideController : ControllerBase
{
    private readonly DataContext _context;

    public RideController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("AddDriverRide")]
    public async Task<ActionResult<RideDto>> AddRide(AddRideDto addRideDto)
    {
        var user = await _context
            .Users
            .Include(u => u.Driver)
            .SingleOrDefaultAsync(u => u.Username.ToLower() == addRideDto.Username.ToLower());

        if (user == null) return NotFound("User not found");

        if (user.Driver == null) return BadRequest("User is not a driver");

        // check if there are any other rides for the same day
        Ride? ride = await _context
                        .Rides
                        .SingleOrDefaultAsync(r => r.Driver == user.Driver && r.day == addRideDto.day);
        if (ride != null) return BadRequest("This day is already booked");

        // add ride
        var newRide = new Ride
        {
            Driver = user.Driver,
            day = addRideDto.day,
            startLocation = addRideDto.SourceLocation,
            endLocation = addRideDto.DestinationLocation
        };
        _context.Rides.Add(newRide);
        await _context.SaveChangesAsync();

        return new RideDto
        {
            Day = newRide.day,
            Driverusername = newRide.Driver.User.Username,
            EndLocation = newRide.endLocation,
            StartLocation = newRide.startLocation,
            RideId = newRide.Id
        };
    }
}
