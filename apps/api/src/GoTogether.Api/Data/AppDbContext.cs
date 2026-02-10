using Microsoft.EntityFrameworkCore;
using GoTogether.Entities;

namespace GoTogether.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Place> Places => Set<Place>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<EventAttendee> EventAttendees => Set<EventAttendee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---- Users ----
        modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.Auth0Sub)
            .IsUnique();

        // ---- Place ----
        modelBuilder.Entity<Place>(entity =>
        {
            entity.HasKey(p => p.Id);

            // Helpful indexes
            entity.HasIndex(p => new { p.OwnerUserId, p.Name }); // for "my places" + (optional) unique name per user
            entity.HasIndex(p => p.OwnerUserId);

            entity.Property(p => p.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(p => p.Description)
                .HasMaxLength(2000);
        });

        // ---- Event ----
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(e => e.Description)
                .HasMaxLength(4000);

            entity.Property(e => e.TimeZoneId)
                .HasMaxLength(64);

            // Relationships
            entity.HasOne(e => e.Place)
                .WithMany() // Place doesn't need navigation to Events yet
                .HasForeignKey(e => e.PlaceId)
                .OnDelete(DeleteBehavior.Restrict); // prevent deleting Place if Events exist

            entity.HasMany(e => e.Attendees)
                .WithOne(a => a.Event)
                .HasForeignKey(a => a.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // Helpful indexes
            entity.HasIndex(e => new { e.OrganizerUserId, e.StartsAt });
            entity.HasIndex(e => new { e.PlaceId, e.StartsAt });
        });

                // ---- EventAttendee ----
        modelBuilder.Entity<EventAttendee>(entity =>
        {
            // Composite PK prevents duplicate attendees per event
            entity.HasKey(a => new { a.EventId, a.UserId });

            entity.Property(a => a.UserId)
                .HasMaxLength(128)
                .IsRequired();

            // Helpful indexes
            entity.HasIndex(a => a.UserId); // "events I'm invited to" queries

        });

    }
}