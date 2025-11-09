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
            entity.Property(e => e.RecipientUserName)
                .HasMaxLength(50);
        });

        var chatUsers = modelBuilder.Entity<User>();
        chatUsers.ToTable("ChatUsers");
        chatUsers.Property(u => u.IdentityUserId).HasMaxLength(450);
        chatUsers.HasIndex(u => u.IdentityUserId)
            .IsUnique()
            .HasFilter("[IdentityUserId] IS NOT NULL");

        chatUsers.HasData(
            new User { Id = 1, Name = "James Kirk", Email = "kirk@enterprise.com", Role = "Captain" },
            new User { Id = 2, Name = "Spock", Email = "spock@enterprise.com", Role = "Science Officer" },
            new User { Id = 3, Name = "Leonard McCoy", Email = "bones@enterprise.com", Role = "Chief Medical Officer" },
            new User { Id = 4, Name = "Nyota Uhura", Email = "uhura@enterprise.com", Role = "Communications Officer" },
            new User { Id = 5, Name = "Montgomery Scott", Email = "scotty@enterprise.com", Role = "Chief Engineer" }
        );
    }
}
