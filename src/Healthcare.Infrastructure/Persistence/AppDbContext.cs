using Healthcare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Healthcare.Infrastructure.Persistence
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options){}

        public DbSet<Patient> Patient { get; set; }
        public DbSet<User> User { get; set; } 
        public DbSet<AuditLog> AuditLog { get; set; }
        public DbSet<WebhookEndpoint> WebhookEndpoints => Set<WebhookEndpoint>();

        public DbSet<WebhookDeliveryLog> WebhookDeliveryLogs => Set<WebhookDeliveryLog>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique();
        }
    }
}
