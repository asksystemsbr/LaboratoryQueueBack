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
            // Configuração para QueueServiceType
            modelBuilder.Entity<QueueServiceType>()
                .ToTable("queue_service_type", "dbo")
                .Property(q => q.CreatedAt)
                .HasColumnName("created_at");

            modelBuilder.Entity<QueueServiceType>()
                .Property(q => q.UpdatedAt)
                .HasColumnName("updated_at");

            // Configuração para QueueTicket
            modelBuilder.Entity<QueueTicket>()
                .ToTable("queue_ticket", "dbo")
                .Property(q => q.ServiceTypeId)
                .HasColumnName("service_type_id");

            modelBuilder.Entity<QueueTicket>()
                .Property(q => q.CounterId)
                .HasColumnName("counter_id");

            modelBuilder.Entity<QueueTicket>()
                .Property(q => q.CreatedAt)
                .HasColumnName("created_at");

            modelBuilder.Entity<QueueTicket>()
                .Property(q => q.IssuedAt)
                .HasColumnName("issued_at");

            modelBuilder.Entity<QueueTicket>()
                .Property(q => q.CalledAt)
                .HasColumnName("called_at");

            modelBuilder.Entity<QueueTicket>()
                .Property(q => q.UpdatedAt)
                .HasColumnName("updated_at");

            modelBuilder.Entity<QueueTicket>()
                .HasOne(t => t.ServiceType)
                .WithMany()
                .HasForeignKey(t => t.ServiceTypeId);

            modelBuilder.Entity<QueueTicket>()
                .HasOne(t => t.Counter)
                .WithMany()
                .HasForeignKey(t => t.CounterId);

            modelBuilder.Entity<QueueTicket>()
                 .Property(t => t.PrintStatus)
                 .HasDefaultValue("PENDING");

            modelBuilder.Entity<QueueTicket>()
                 .Property(e => e.PrintStatus)
                 .HasColumnName("print_status");

            // Configuração para QueueCounter
            modelBuilder.Entity<QueueCounter>()
                .ToTable("queue_counter", "dbo")
                .Property(q => q.CreatedAt)
                .HasColumnName("created_at");

            modelBuilder.Entity<QueueCounter>()
                .Property(q => q.UpdatedAt)
                .HasColumnName("updated_at");

            // Configuração para QueueConfig
            modelBuilder.Entity<QueueConfig>()
                .ToTable("queue_config", "dbo");
        }
    }
}
