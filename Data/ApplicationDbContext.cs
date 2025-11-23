
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
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<TrainerShift> TrainerShifts { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<MembershipPlans> MembershipPlans { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure TrainerShift relationships
            builder.Entity<TrainerShift>()
                .HasOne(ts => ts.Trainer)
                .WithMany()
                .HasForeignKey(ts => ts.TrainerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TrainerShift>()
                .HasOne(ts => ts.Shift)
                .WithMany()
                .HasForeignKey(ts => ts.ShiftId)
                .OnDelete(DeleteBehavior.Cascade);

            // Plan relationship
            builder.Entity<Plan>()
                .HasOne(p => p.Member)
                .WithMany()
                .HasForeignKey(p => p.MemberId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
