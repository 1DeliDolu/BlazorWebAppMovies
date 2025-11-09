using BlazorSignalRApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorSignalRApp.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> ChatUsers => Set<User>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();

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

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.ToTable("ChatMessages");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Message)
                .IsRequired()
                .HasMaxLength(500);
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("GETUTCDATE()");
        });
    }
}
