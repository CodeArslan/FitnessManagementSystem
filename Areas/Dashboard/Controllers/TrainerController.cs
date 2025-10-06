using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FitnessManagementSystem.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    //[Authorize(Roles = "Trainer")]
    public class TrainerController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Trainer Dashboard";
            ViewData["Description"] = "Manage your clients and schedules";
            return View();
        }

        public IActionResult WorkoutPlans()
        {
            ViewData["Title"] = "Workout Plan Management";
            ViewData["Description"] = "Create and modify workout plans for members";
            return View();
        }

        public IActionResult DietPlans()
        {
            ViewData["Title"] = "Diet Plan Management";
            ViewData["Description"] = "Provide dietary recommendations for members";
            return View();
        }

        public IActionResult MarkAttendance()
        {
            ViewData["Title"] = "Mark Attendance";
            ViewData["Description"] = "Record and monitor member attendance";
            return View();
        }

        public IActionResult ClassScheduling()
        {
            ViewData["Title"] = "Class Scheduling";
            ViewData["Description"] = "Manage your class schedules and availability";
            return View();
        }

        public IActionResult MemberProgress()
        {
            ViewData["Title"] = "Member Progress";
            ViewData["Description"] = "View progress reports of members";
            return View();
        }
    }
}