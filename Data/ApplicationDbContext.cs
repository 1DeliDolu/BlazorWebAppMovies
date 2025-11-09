using Microsoft.EntityFrameworkCore;
using BlazorSignalRApp.Models;

namespace BlazorSignalRApp.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "James Kirk", Email = "kirk@enterprise.com", Role = "Captain" },
            new User { Id = 2, Name = "Spock", Email = "spock@enterprise.com", Role = "Science Officer" },
            new User { Id = 3, Name = "Leonard McCoy", Email = "bones@enterprise.com", Role = "Chief Medical Officer" },
            new User { Id = 4, Name = "Nyota Uhura", Email = "uhura@enterprise.com", Role = "Communications Officer" },
            new User { Id = 5, Name = "Montgomery Scott", Email = "scotty@enterprise.com", Role = "Chief Engineer" }
        );
    }
}
