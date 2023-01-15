using CarpoolAPI.Features.Driver;
using CarpoolAPI.Features.Ride;
using CarpoolAPI.Features.User;
using Microsoft.EntityFrameworkCore;

namespace CarpoolAPI;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to sqlite database
        options.UseSqlite(Configuration.GetConnectionString("WebApiDatabase"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
           .HasOne(u => u.Driver)
           .WithOne(d => d.User)
           .HasForeignKey<User>(d => d.DriverId);

        modelBuilder.Entity<Ride>()
            .HasOne(r => r.Driver)
            .WithMany(d => d.Rides)
            .HasForeignKey(r => r.DriverId);

        modelBuilder.Entity<Ride>()
            .HasMany(r => r.Users)
            .WithMany(u => u.Rides);

        //modelBuilder.Entity<Ride>()
        //    .Ignore(r => r.Users);

        //modelBuilder.Entity<Ride>()
        //    .Ignore(r => r.Driver);

        //modelBuilder.Entity<Driver>()
        //    .Ignore(r => r.User);

        //modelBuilder.Entity<Driver>()
        //    .Ignore(r => r.Rides);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Ride> Rides { get; set; }
}