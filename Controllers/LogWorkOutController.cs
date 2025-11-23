using FitnessManagementSystem.Data;
using FitnessManagementSystem.Models;
using FitnessManagementSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;

namespace FitnessManagementSystem.Controllers
{
    public class LogWorkOutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public LogWorkOutController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> LogWorkout(DateTime date, int duration, int calories, string notes)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var workout = new WorkoutSession
            {
                UserId = user.Id,
                SessionDate = date,
                DurationMinutes = duration,
                CaloriesBurned = calories,
                Notes = notes,
                CreatedAt = DateTime.UtcNow
            };

            _context.WorkoutSessions.Add(workout);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ViewProgress));
        }

        [HttpGet]
        public IActionResult LogWorkout()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LogProgress()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ViewProgress()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var progressRecords = await _context.ProgressRecords
                .Where(r => r.UserId == user.Id)
                .OrderByDescending(r => r.RecordedAt)
                .ToListAsync();

            var workoutSessions = await _context.WorkoutSessions
                .Where(w => w.UserId == user.Id)
                .OrderByDescending(w => w.SessionDate)
                .ToListAsync();

            var viewModel = new MemberProgressViewModel
            {
                ProgressRecords = progressRecords,
                WorkoutSessions = workoutSessions
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> LogProgress(float weight, float bmi, float bodyFat)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var record = new ProgressRecord
            {
                UserId = user.Id,
                Weight = weight,
                BMI = bmi,
                BodyFatPercentage = bodyFat,
                RecordedAt = DateTime.UtcNow
            };

            _context.ProgressRecords.Add(record);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ViewProgress));
        }

        [HttpGet]
        public async Task<IActionResult> ViewMemberProgress()
        {
            // 1️⃣ Get the latest workout session to extract the UserId
            var lastSession = await _context.WorkoutSessions
                .OrderByDescending(w => w.SessionDate)
                .FirstOrDefaultAsync();

            if (lastSession == null)
            {
                ViewBag.MemberName = "Member";
                return View(new MemberProgressViewModel());
            }

            string memberId = lastSession.UserId;  // 🔥 This is the UserId you want!

            // 2️⃣ Get member details
            var member = await _userManager.FindByIdAsync(memberId);

            // 3️⃣ Load records only for this user
            var progressRecords = await _context.ProgressRecords
                .Where(r => r.UserId == memberId)
                .OrderByDescending(r => r.RecordedAt)
                .ToListAsync();

            var workoutSessions = await _context.WorkoutSessions
                .Where(w => w.UserId == memberId)
                .OrderByDescending(w => w.SessionDate)
                .ToListAsync();

            // 4️⃣ Set the name
            ViewBag.MemberName = $"{member?.FirstName} {member?.LastName}".Trim();

            var viewModel = new MemberProgressViewModel
            {
                ProgressRecords = progressRecords,
                WorkoutSessions = workoutSessions
            };

            return View(viewModel);
        }
    }
}
