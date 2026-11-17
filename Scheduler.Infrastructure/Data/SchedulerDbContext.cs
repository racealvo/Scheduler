using Microsoft.EntityFrameworkCore;
using Scheduler.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Infrastructure.Data
{
    public class SchedulerDbContext: DbContext
    {
        SchedulerDbContext(DbContextOptions<SchedulerDbContext> dbContextOptions) : base(dbContextOptions) {}

        public DbSet<Therapist> Therapists => Set<Therapist>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<Reminder> Reminders => Set<Reminder>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SchedulerDbContext).Assembly);
        }
    }
}
