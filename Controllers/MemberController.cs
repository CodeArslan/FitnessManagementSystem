using FMS.Data;
using FMS.Models;
using FMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FMS.Controllers;

[Authorize(Roles = "Member")]
public class MemberController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public MemberController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Dashboard()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        var memberProfile = await _context.MemberProfiles
            .FirstOrDefaultAsync(m => m.MemberId == user.Id);

        if (memberProfile == null)
        {
            // Handle case where profile is missing (shouldn't happen with new flow)
            return RedirectToAction("Index", "Home");
        }

        // Load other data like memberships, appointments, etc.
        var activeMembership = await _context.UserMemberships
            .Include(m => m.Plan)
            .Where(m => m.UserId == user.Id && m.Status == "Active" && m.EndDate > DateTime.UtcNow)
            .OrderByDescending(m => m.EndDate)
            .FirstOrDefaultAsync();

        var upcomingAppointments = await _context.Appointments
            .Include(a => a.Trainer)
            .Where(a => a.UserId == user.Id && a.SessionDate > DateTime.UtcNow && a.Status != "Cancelled")
            .OrderBy(a => a.SessionDate)
            .Take(5)
            .ToListAsync();

        var viewModel = new MemberDashboardViewModel
        {
            Profile = memberProfile,
            ActiveMembership = activeMembership,
            UpcomingAppointments = upcomingAppointments
        };

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> PurchasePlan()
    {
        var plans = await _context.MembershipPlans.Where(p => p.IsActive).ToListAsync();
        return View(plans);
    }

    [HttpPost]
    public async Task<IActionResult> PurchasePlan(int planId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var plan = await _context.MembershipPlans.FindAsync(planId);
        if (plan == null) return NotFound();

        var membership = new UserMembership
        {
            UserId = user.Id,
            PlanId = planId,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(plan.DurationDays),
            Status = "Active",
            CreatedAt = DateTime.UtcNow
        };
        _context.UserMemberships.Add(membership);
        await _context.SaveChangesAsync();

        return RedirectToAction("Dashboard");
    }

    [HttpGet]
    public async Task<IActionResult> BookSession()
    {
        var trainers = await _userManager.GetUsersInRoleAsync("Trainer");
        return View(trainers);
    }

    [HttpPost]
    public async Task<IActionResult> BookSession(string trainerId, DateTime date, string type)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var appointment = new Appointment
        {
            UserId = user.Id,
            TrainerId = trainerId,
            SessionDate = date,
            SessionType = type,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();
        return RedirectToAction("Dashboard");
    }

    [HttpGet]
    public async Task<IActionResult> ViewProgress()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var records = await _context.ProgressRecords
            .Where(r => r.UserId == user.Id)
            .OrderByDescending(r => r.RecordedAt)
            .ToListAsync();

        return View(records);
    }

    [HttpGet]
    public async Task<IActionResult> GiveFeedback()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        // Get completed appointments to rate
        var completedAppointments = await _context.Appointments
            .Include(a => a.Trainer)
            .Where(a => a.UserId == user.Id && a.Status == "Completed") // Assuming "Completed" status exists
            .ToListAsync();

        return View(completedAppointments);
    }

    [HttpPost]
    public async Task<IActionResult> GiveFeedback(int appointmentId, int rating, string comment)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var appointment = await _context.Appointments.Include(a => a.Trainer).FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
        if (appointment == null || appointment.UserId != user.Id) return NotFound();

        var feedback = new Feedback
        {
            UserId = user.Id,
            TrainerId = appointment.TrainerId,
            AppointmentId = appointmentId,
            Rating = rating,
            Comment = comment,
            CreatedAt = DateTime.UtcNow
        };

        _context.Feedbacks.Add(feedback);
        await _context.SaveChangesAsync();

        return RedirectToAction("Dashboard");
    }
    [HttpGet]
    public async Task<IActionResult> MyPlans()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var workoutPlans = await _context.WorkoutPlans
            .Include(w => w.Trainer)
            .Where(w => w.MemberId == user.Id)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync();

        var dietPlans = await _context.DietPlans
            .Include(d => d.Trainer)
            .Where(d => d.MemberId == user.Id)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync();

        var viewModel = new MemberPlansViewModel
        {
            WorkoutPlans = workoutPlans,
            DietPlans = dietPlans
        };

        return View(viewModel);
    }
}
