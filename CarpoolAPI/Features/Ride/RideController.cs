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

    [HttpGet("GetRides")]
    public async Task<ActionResult<List<RideDto>>> GetRides(
        string? ignoreUserRides, DayOfWeek? dayOfWeek, string? sourceLocation, string? destinationLocation)
    {
        var rides = _context
            .Rides
            .Include(r => r.Driver)
            .Include(r => r.Users)
            .Where(r => r.Users.Count() < r.Driver.seatCount); // ignore full rides
        if (ignoreUserRides != null)
            rides = rides.Where(r => !r.Users.Any(u => u.Username == ignoreUserRides));
        if (dayOfWeek != null)
            rides = rides.Where(r => r.day == dayOfWeek);
        if (sourceLocation != null)
            rides = rides.Where(r => r.startLocation == sourceLocation);
        if (destinationLocation != null)
            rides = rides.Where(r => r.endLocation == destinationLocation);
        return await rides
            .Select(r => new RideDto
            {
                Day = r.day,
                Driverusername = r.Driver.User.Username,
                EndLocation = r.endLocation,
                StartLocation = r.startLocation,
                RideId = r.Id
            })
            .ToListAsync();
    }

    [HttpDelete("DeleteRide")]
    public async Task<IActionResult> Delete([FromQuery] string username, [FromQuery] int rideId)
    {
        // check user exists
        var user = await _context.Users.Include(u => u.Driver).SingleOrDefaultAsync(u => u.Username == username);
        if (user == null) return NotFound("User not found");

        // check user is a driver
        if (user.DriverId == null) return BadRequest("User is not a driver");

        // check ride exists
        var ride = await _context.Rides.Include(r => r.Driver).SingleOrDefaultAsync(r => r.Id == rideId);
        if (ride == null) return NotFound("Ride not found");

        // check ride belongs to user
        if (ride.DriverId != user.DriverId) return BadRequest("Ride doesn't belong to driver");

        // delete ride
        _context.Remove(ride);

        await _context.SaveChangesAsync();
        return Ok();
    }
}
