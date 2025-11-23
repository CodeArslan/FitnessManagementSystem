using FitnessManagementSystem.Data;
using FitnessManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessManagementSystem.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public AppointmentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> BookAppointment()
        {
            var trainers = await _context.Users
                .Where(c => (c.Role ?? "").ToLower() == "trainer")
                .ToListAsync();

            return View(trainers);
        }


        [HttpPost]
        public async Task<IActionResult> BookAppointment(string trainerId, DateTime date, string type)
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
            return RedirectToAction("BookAppointment");
        }
    }
}
