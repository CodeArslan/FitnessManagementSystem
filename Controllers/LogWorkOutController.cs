using FitnessManagementSystem.Data;
using FitnessManagementSystem.Models;
using FitnessManagementSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    }
}
