using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FitnessManagementSystem.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Admin Dashboard";
            ViewData["Description"] = "Manage your gym efficiently";
            return View();
        }

        public IActionResult ManageMembers()
        {
            ViewData["Title"] = "Manage Members";
            ViewData["Description"] = "Add, edit, and delete member records";
            return View();
        }

        public IActionResult ManageTrainers()
        {
            ViewData["Title"] = "Manage Trainers";
            ViewData["Description"] = "Add, update, and remove trainers";
            return View();
        }

        public IActionResult TrainerScheduling()
        {
            ViewData["Title"] = "Trainer Scheduling";
            ViewData["Description"] = "Assign trainers to shifts and classes";
            return View();
        }

        public IActionResult MonitorFeedback()
        {
            ViewData["Title"] = "Monitor Feedback";
            ViewData["Description"] = "View ratings and reviews for trainers";
            return View();
        }
    }
}