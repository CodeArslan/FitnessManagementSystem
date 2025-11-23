using FMS.Data;
using FMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FMS.Controllers;

[Authorize(Roles = "Trainer")]
public class TrainerController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public TrainerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Dashboard()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        // Fetch upcoming appointments
        var appointments = await _context.Appointments
            .Include(a => a.User)
            .Where(a => a.TrainerId == user.Id && a.Status == "Pending")
            .OrderBy(a => a.SessionDate)
            .ToListAsync();

        return View(appointments);
    }

    [HttpGet]
    public async Task<IActionResult> ManageWorkoutPlan()
    {
        // List members assigned to this trainer (via appointments or direct assignment logic)
        // For simplicity, let's list all members
        var members = await _userManager.GetUsersInRoleAsync("Member");
        return View(members);
    }

    [HttpPost]
    public async Task<IActionResult> CreateWorkoutPlan(string memberId, string details, DateTime startDate, DateTime endDate)
    {
        var trainer = await _userManager.GetUserAsync(User);
        var plan = new WorkoutPlan
        {
            TrainerId = trainer.Id,
            MemberId = memberId,
            PlanDetails = details,
            StartDate = startDate,
            EndDate = endDate,
            CreatedAt = DateTime.UtcNow
        };
        _context.WorkoutPlans.Add(plan);
        await _context.SaveChangesAsync();
        return RedirectToAction("Dashboard");
    }

    [HttpGet]
    public async Task<IActionResult> ManageDietPlan()
    {
        var members = await _userManager.GetUsersInRoleAsync("Member");
        return View(members);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDietPlan(string memberId, string details, DateTime startDate, DateTime endDate)
    {
        var trainer = await _userManager.GetUserAsync(User);
        var plan = new DietPlan
        {
            TrainerId = trainer.Id,
            MemberId = memberId,
            DietDetails = details,
            StartDate = startDate,
            EndDate = endDate,
            CreatedAt = DateTime.UtcNow
        };
        _context.DietPlans.Add(plan);
        await _context.SaveChangesAsync();
        return RedirectToAction("Dashboard");
    }

    [HttpPost]
    public async Task<IActionResult> MarkAttendance(int appointmentId, string status)
    {
        var appointment = await _context.Appointments.FindAsync(appointmentId);
        if (appointment != null)
        {
            appointment.Status = status;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Dashboard");
    }

    [HttpGet]
    public async Task<IActionResult> ViewMemberProgress(string memberId)
    {
        var records = await _context.ProgressRecords
            .Where(r => r.UserId == memberId)
            .OrderByDescending(r => r.RecordedAt)
            .ToListAsync();
        
        ViewBag.MemberId = memberId;
        return View(records);
    }
    [HttpGet]
    public async Task<IActionResult> MySchedule()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var shifts = await _context.TrainerShifts
            .Where(s => s.TrainerId == user.Id)
            .OrderBy(s => s.DayOfWeek)
            .ThenBy(s => s.StartTime)
            .ToListAsync();

        return View(shifts);
    }
}
