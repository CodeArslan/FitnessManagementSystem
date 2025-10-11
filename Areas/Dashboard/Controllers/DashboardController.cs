using Microsoft.AspNetCore.Mvc;

namespace FitnessManagementSystem.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    ViewData["Title"] = "Admin Dashboard";
                    ViewData["Description"] = "Manage your gym efficiently";
                }
                else if (User.IsInRole("Trainer"))
                {
                    ViewData["Title"] = "Trainer Dashboard";
                    ViewData["Description"] = "Manage your clients and schedules";
                }
                else
                {
                    ViewData["Title"] = "Dashboard";
                    ViewData["Description"] = "Welcome to your dashboard";
                }
            }
            else
            {
                ViewData["Title"] = "Gym Management System";
                ViewData["Description"] = "Welcome! Please login to access all features";
            }

            return View();
        }

        // ============ ADMIN FUNCTIONS ============
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

        public IActionResult ViewFeedback()
        {
            ViewData["Title"] = "Monitor Feedback";
            ViewData["Description"] = "View ratings and reviews for trainers";
            return View();
        }

        public IActionResult Reports()
        {
            ViewData["Title"] = "Reports";
            ViewData["Description"] = "View gym performance reports";
            return View();
        }

        // ============ TRAINER FUNCTIONS ============
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

        // ============ COMMON FUNCTIONS ============
        public IActionResult Profile()
        {
            ViewData["Title"] = "My Profile";
            ViewData["Description"] = "Manage your account information";
            return View();
        }
    }
}