using FitnessManagementSystem.Data;
using FitnessManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessManagementSystem.Controllers
{
    public class TrainerAvailabilityController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public TrainerAvailabilityController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> ManageAvailability()
        {
            var trainer = await _userManager.GetUserAsync(User);
            if (trainer == null) return Challenge();

            var availabilities = await _context.TrainerAvailabilitys
                .Where(a => a.TrainerId == trainer.Id)
                .OrderBy(a => a.DayOfWeek)
                .ThenBy(a => a.StartTime)
                .ToListAsync();

            return View(availabilities);
        }

        [HttpPost]
        public async Task<IActionResult> AddAvailability(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
        {
            var trainer = await _userManager.GetUserAsync(User);
            if (trainer == null) return Challenge();

            var availability = new TrainerAvailability
            {
                TrainerId = trainer.Id,
                DayOfWeek = dayOfWeek,
                StartTime = startTime,
                EndTime = endTime
            };

            _context.TrainerAvailabilitys.Add(availability);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageAvailability));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAvailability(int id)
        {
            var availability = await _context.TrainerAvailabilitys.FindAsync(id);
            if (availability != null)
            {
                var trainer = await _userManager.GetUserAsync(User);
                if (trainer != null && availability.TrainerId == trainer.Id)
                {
                    _context.TrainerAvailabilitys.Remove(availability);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(ManageAvailability));
        }
    }
}
