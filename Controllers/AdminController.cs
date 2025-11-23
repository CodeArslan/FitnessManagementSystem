using FMS.Data;
using FMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FMS.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index()
    {
        var totalMembers = await _userManager.GetUsersInRoleAsync("Member");
        var totalTrainers = await _userManager.GetUsersInRoleAsync("Trainer");
        var totalAppointments = await _context.Appointments.CountAsync();
        var totalRevenue = await _context.UserMemberships.SumAsync(m => m.Plan.Price);

        ViewBag.TotalMembers = totalMembers.Count;
        ViewBag.TotalTrainers = totalTrainers.Count;
        ViewBag.TotalAppointments = totalAppointments;
        ViewBag.TotalRevenue = totalRevenue;

        return View();
    }

    // --- Manage Members ---
    public async Task<IActionResult> ManageMembers()
    {
        var members = await _userManager.GetUsersInRoleAsync("Member");
        return View(members);
    }

    public async Task<IActionResult> DeleteMember(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            await _userManager.DeleteAsync(user);
        }
        return RedirectToAction(nameof(ManageMembers));
    }

    [HttpGet]
    public IActionResult CreateMember()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateMember(string email, string password, string name, int age, float height, float weight)
    {
        var user = new ApplicationUser { UserName = email, Email = email, FullName = name, EmailConfirmed = true };
        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Member");
            var profile = new MemberProfile 
            { 
                MemberId = user.Id, 
                Age = age,
                HeightCm = height,
                WeightKg = weight,
                CreatedAt = DateTime.UtcNow 
            };
            _context.MemberProfiles.Add(profile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageMembers));
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }
        return View();
    }

    // --- Manage Trainers ---
    public async Task<IActionResult> ManageTrainers()
    {
        var trainers = await _userManager.GetUsersInRoleAsync("Trainer");
        return View(trainers);
    }

    [HttpGet]
    public IActionResult CreateTrainer()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateTrainer(string email, string password, string name)
    {
        var user = new ApplicationUser { UserName = email, Email = email, FullName = name, EmailConfirmed = true };
        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Trainer");
            var profile = new TrainerProfile { TrainerId = user.Id, CreatedAt = DateTime.UtcNow };
            _context.TrainerProfiles.Add(profile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageTrainers));
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }
        return View();
    }

    public async Task<IActionResult> DeleteTrainer(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            await _userManager.DeleteAsync(user);
        }
        return RedirectToAction(nameof(ManageTrainers));
    }

    // --- Trainer Scheduling ---
    public async Task<IActionResult> ManageShifts(string trainerId)
    {
        var trainer = await _userManager.FindByIdAsync(trainerId);
        if (trainer == null) return NotFound();

        var shifts = await _context.TrainerShifts.Where(s => s.TrainerId == trainerId).ToListAsync();
        ViewBag.TrainerId = trainerId;
        ViewBag.TrainerName = trainer.UserName;
        return View(shifts);
    }

    [HttpPost]
    public async Task<IActionResult> AddShift(string trainerId, DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
    {
        var shift = new TrainerShift
        {
            TrainerId = trainerId,
            DayOfWeek = dayOfWeek,
            StartTime = startTime,
            EndTime = endTime
        };
        _context.TrainerShifts.Add(shift);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(ManageShifts), new { trainerId });
    }

    public async Task<IActionResult> DeleteShift(int id)
    {
        var shift = await _context.TrainerShifts.FindAsync(id);
        if (shift != null)
        {
            _context.TrainerShifts.Remove(shift);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageShifts), new { trainerId = shift.TrainerId });
        }
        return RedirectToAction(nameof(ManageTrainers));
    }

    // --- Monitor Feedback ---
    public async Task<IActionResult> ViewFeedback()
    {
        var feedback = await _context.Feedbacks
            .Include(f => f.User)
            .Include(f => f.Appointment)
            .ThenInclude(a => a.Trainer)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();
        return View(feedback);
    }
}
