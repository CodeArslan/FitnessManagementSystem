using FMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FMS.Data;

public static class DbInitializer
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        using var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        using var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        context.Database.EnsureCreated();

        // Seed Roles
        string[] roles = { "Admin", "Trainer", "Member" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Seed Membership Plans
        if (!context.MembershipPlans.Any())
        {
            var plans = new List<MembershipPlan>
            {
                new MembershipPlan
                {
                    PlanName = "Silver",
                    DurationDays = 30,
                    Price = 29.99m,
                    Description = "Basic access to gym facilities.",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new MembershipPlan
                {
                    PlanName = "Gold",
                    DurationDays = 90,
                    Price = 79.99m,
                    Description = "Access to gym + 5 trainer sessions.",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new MembershipPlan
                {
                    PlanName = "Diamond",
                    DurationDays = 365,
                    Price = 299.99m,
                    Description = "All access + unlimited trainer sessions + diet plan.",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };
            context.MembershipPlans.AddRange(plans);
            await context.SaveChangesAsync();
        }

        // Seed Admin User (Optional but good for testing)
        if (await userManager.FindByEmailAsync("admin@fms.com") == null)
        {
            var admin = new ApplicationUser
            {
                UserName = "admin@fms.com",
                Email = "admin@fms.com",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(admin, "Admin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }

        // Seed Trainer User
        if (await userManager.FindByEmailAsync("trainer@fms.com") == null)
        {
            var trainer = new ApplicationUser
            {
                UserName = "trainer@fms.com",
                Email = "trainer@fms.com",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(trainer, "Trainer@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(trainer, "Trainer");
                
                // Create Trainer Profile
                var trainerProfile = new TrainerProfile
                {
                    TrainerId = trainer.Id,
                    AverageRating = 5.0,
                    RatingCount = 1,
                    UpdatedAt = DateTime.UtcNow
                };
                context.TrainerProfiles.Add(trainerProfile);
                await context.SaveChangesAsync();
            }
        }
    }
}
