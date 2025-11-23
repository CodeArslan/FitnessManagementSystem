
using FitnessManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace FitnessManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<TrainerShift> TrainerShifts { get; set; }
        public DbSet<Plan> Plans { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Attendance relationships
            builder.Entity<Attendance>()
                .HasOne(a => a.Member)
                .WithMany()
                .HasForeignKey(a => a.MemberId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

            builder.Entity<Attendance>()
                .HasOne(a => a.Trainer)
                .WithMany()
                .HasForeignKey(a => a.TrainerId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

            // TrainerShift relationships
            builder.Entity<TrainerShift>()
                .HasOne(ts => ts.Trainer)
                .WithMany()
                .HasForeignKey(ts => ts.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TrainerShift>()
                .HasOne(ts => ts.Shift)
                .WithMany()
                .HasForeignKey(ts => ts.ShiftId)
                .OnDelete(DeleteBehavior.Restrict);

            // Plan relationship
            builder.Entity<Plan>()
                .HasOne(p => p.Member)
                .WithMany()
                .HasForeignKey(p => p.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
