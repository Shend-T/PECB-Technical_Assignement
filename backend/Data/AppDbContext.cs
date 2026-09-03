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
    public DbSet<Ticket> Tickets => Set<Ticket>();

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

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(t => t.id);

            entity.HasIndex(t => t.referenceId)
                .IsUnique();

            entity.Property(t => t.referenceId)
                .IsRequired();

            entity.Property(t => t.title)
                .IsRequired();

            entity.Property(t => t.description)
                .IsRequired();

            entity.Property(t => t.customerName)
                .IsRequired();

            entity.Property(t => t.customerEmail)
                .IsRequired();

            entity.Property(t => t.priority)
                .IsRequired()
                .HasDefaultValue(Priority.Low);
            
            entity.Property(t => t.status)
                .IsRequired()
                .HasDefaultValue(Status.New);

            entity.Property(t => t.createdDate)
                .IsRequired();
                
            entity.Property(t => t.lastModifiedDate)
                .IsRequired();
            
            entity.Property(t => t.resolvedDate);
            entity.Property(t => t.closedDate);

            entity.Property(t => t.dueDate)
                .IsRequired();

            // Agent 1:N Tickets
            entity.HasOne(t => t.assignedAgent)
                .WithMany()
                .HasForeignKey(t => t.assignedAgentId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}