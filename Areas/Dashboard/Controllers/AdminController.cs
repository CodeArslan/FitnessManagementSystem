using FitnessManagementSystem.Data;
using FitnessManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessManagementSystem.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        //public async Task<IActionResult> Index()
        //{
        //    var feedback = await _context.Feedbacks
        //    .Include(f => f.User)
        //    .Include(f => f.Shift)
        //    .OrderByDescending(f => f.CreatedAt)
        //    .ToListAsync();

        //    return View(feedback);
        //}
    }
}
