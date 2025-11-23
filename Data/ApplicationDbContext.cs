using FMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FMS.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<MembershipPlan> MembershipPlans { get; set; }
    public DbSet<UserMembership> UserMemberships { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<WorkoutSession> WorkoutSessions { get; set; }
    public DbSet<ProgressRecord> ProgressRecords { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<TrainerProfile> TrainerProfiles { get; set; }
    public DbSet<MemberProfile> MemberProfiles { get; set; }
    public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
    public DbSet<DietPlan> DietPlans { get; set; }
    public DbSet<TrainerShift> TrainerShifts { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Prevent cascade delete from AspNetUsers to business data
        builder.Entity<UserMembership>()
            .HasOne(um => um.User)
            .WithMany()
            .HasForeignKey(um => um.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Payment>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Appointment>()
            .HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Appointment>()
            .HasOne(a => a.Trainer)
            .WithMany()
            .HasForeignKey(a => a.TrainerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Feedback>()
            .HasOne(f => f.User)
            .WithMany()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Feedback>()
            .HasOne(f => f.Trainer)
            .WithMany()
            .HasForeignKey(f => f.TrainerId)
            .OnDelete(DeleteBehavior.Restrict);

        // TrainerProfile PK is TrainerId
        builder.Entity<TrainerProfile>()
            .HasKey(t => t.TrainerId);

        // MemberProfile PK is MemberId
        builder.Entity<MemberProfile>()
            .HasKey(m => m.MemberId);

        // Prevent cascade cycles for WorkoutPlan
        builder.Entity<WorkoutPlan>()
            .HasOne(wp => wp.Trainer)
            .WithMany()
            .HasForeignKey(wp => wp.TrainerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<WorkoutPlan>()
            .HasOne(wp => wp.Member)
            .WithMany()
            .HasForeignKey(wp => wp.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        // Prevent cascade cycles for DietPlan
        builder.Entity<DietPlan>()
            .HasOne(dp => dp.Trainer)
            .WithMany()
            .HasForeignKey(dp => dp.TrainerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<DietPlan>()
            .HasOne(dp => dp.Member)
            .WithMany()
            .HasForeignKey(dp => dp.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        // Prevent cascade cycles for TrainerShift
        builder.Entity<TrainerShift>()
            .HasOne(ts => ts.Trainer)
            .WithMany()
            .HasForeignKey(ts => ts.TrainerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

