using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Agent> Agents => Set<Agent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agent>(entity =>
        {
            entity.HasKey(a => a.id);

            entity.HasIndex(a => a.email)
                .IsUnique();

            entity.Property(a => a.fullName)
                .IsRequired();

            entity.Property(a => a.email)
                .IsRequired();

            entity.Property(a => a.department)
                .IsRequired()
                .HasDefaultValue(Department.General);

            entity.Property(a => a.active)
                .IsRequired()
                .HasDefaultValue(false);
        });
    }
}