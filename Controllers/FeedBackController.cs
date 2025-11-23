using FitnessManagementSystem.Data;
using FitnessManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessManagementSystem.Controllers
{
    public class FeedBackController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public FeedBackController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> GiveFeedback()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var feedbackGivenIds = await _context.Feedbacks
                .Select(f => f.AppointmentId)
                .ToListAsync();
            var completedAppointments = await _context.Appointments
                .Include(a => a.Trainer)
                .Where(a => a.UserId == user.Id
                            && a.Status == "Completed"
                            && !feedbackGivenIds.Contains(a.AppointmentId))
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

            return RedirectToAction("GiveFeedback");
        }
    }
}
