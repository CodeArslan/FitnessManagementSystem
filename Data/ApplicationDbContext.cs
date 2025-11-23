
using FitnessManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<MembershipPlan> MembershipPlans { get; set; }
        public DbSet<MemberMembership> MemberMembership { get; set; }

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

            builder.Entity<MembershipPlan>().HasData(
            new MembershipPlan
            {
                PlanId = 1,
                PlanName = "Gold",
                DurationMonths = 12,
                Price = 12000M,
                Description = "Full-year plan with trainer and diet support",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new MembershipPlan
            {
                PlanId = 2,
                PlanName = "Silver",
                DurationMonths = 6,
                Price = 7000M,
                Description = "Half-year plan with trainer support",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new MembershipPlan
            {
                PlanId = 3,
                PlanName = "Bronze",
                DurationMonths = 3,
                Price = 4000M,
                Description = "Quarterly plan for new members",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        );


        }

        // Auto-set CreatedAt and UpdatedAt
        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }

        // Async version
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
