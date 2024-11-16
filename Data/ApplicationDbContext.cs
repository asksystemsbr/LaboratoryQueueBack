// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using laboratoryqueue.Models;

namespace laboratoryqueue.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<QueueServiceType> QueueServiceTypes { get; set; }
        public DbSet<QueueTicket> QueueTickets { get; set; }
        public DbSet<QueueCounter> QueueCounters { get; set; }
        public DbSet<QueueConfig> QueueConfigs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QueueServiceType>()
                .ToTable("queue_service_type", "dbo");

            modelBuilder.Entity<QueueTicket>()
                .ToTable("queue_ticket", "dbo")
                .HasOne(t => t.ServiceType)
                .WithMany()
                .HasForeignKey(t => t.ServiceTypeId);

            modelBuilder.Entity<QueueCounter>()
                .ToTable("queue_counter", "dbo");

            modelBuilder.Entity<QueueConfig>()
                .ToTable("queue_config", "dbo");
        }
    }
}