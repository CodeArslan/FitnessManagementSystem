using Microsoft.AspNetCore.Mvc;

namespace FitnessManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register()
        {
            return View();
        }
    }
}
