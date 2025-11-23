using FitnessManagementSystem.Data;
using FitnessManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FitnessManagementSystem.Controllers
{
    public class ClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        public ClassesController(ApplicationDbContext context, IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _config = config;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
