using FitnessManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessManagementSystem.Areas.Dashboard.Controllers
{

    [Area("Dashboard")]
    public class FeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
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
}
