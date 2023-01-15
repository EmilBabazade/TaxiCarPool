using CarpoolAPI.Features.User.Dtos;
using CarpoolAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace CarpoolAPI.Features.User;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;

    public UserController(DataContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        var userExistsByUsername = async (string username)
            => await _context.Users.AnyAsync(u => u.Username == username.ToLower());

        if (await userExistsByUsername(registerDto.Username)) return BadRequest("Username already taken!");

        var userExistsByEmail = async (string email)
            => await _context.Users.AnyAsync(u => u.Email == email.ToLower());
        if (await userExistsByEmail(registerDto.Email)) return BadRequest("Email already taken!");

        using var hmac = new HMACSHA512();

        var user = new User
        {
            Username = registerDto.Username.ToLower(),
            Email = registerDto.Email.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        if (registerDto.IsDriver)
        {
            if (String.IsNullOrWhiteSpace(registerDto.carModel))
                return BadRequest("If user is a driver, carmodel is required");
            if (registerDto.seatCount == null || registerDto.seatCount < 2)
                return BadRequest("If user is a driver, seatcount is required and needs to be more than 1");
            var driver = new Driver.Driver
            {
                carModel = registerDto.carModel,
                seatCount = (int)registerDto.seatCount,
                User = user
            };
            _context.Add(driver);
            user.Driver = driver;
        }

        _context.Add(user);
        await _context.SaveChangesAsync();

        return new UserDto
        {
            Username = user.Username,
            Token = _tokenService.CreateToken(user)
        };
    }

    [HttpPost("Login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        User user = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginDto.Email.ToLower());
        if (user == null) return Unauthorized("Invalid Email");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Password invalid");
        }

        return new UserDto
        {
            Username = user.Username,
            Token = _tokenService.CreateToken(user)
        };
    }

    // TODO: add ride

    // TODO: check if rider going from source to destination locations

    // TODO: check if rider going during the weekdays

    // TODO: check if ride has enough seats

    // TODO: add ride and return ridedto


}
