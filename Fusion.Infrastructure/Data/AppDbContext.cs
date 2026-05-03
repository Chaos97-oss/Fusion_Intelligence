using Fusion.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fusion.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<DeliveryAgent> DeliveryAgents { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>()
            .Property(o => o.Status)
            .HasConversion<string>();

        // Seed data for agents
        modelBuilder.Entity<DeliveryAgent>().HasData(
            new DeliveryAgent { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Alice Speed", IsActive = true },
            new DeliveryAgent { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Bob Quick", IsActive = true }
        );
    }
}
