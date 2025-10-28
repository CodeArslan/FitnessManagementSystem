using FitnessManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FitnessManagementSystem.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class MembersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public MembersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var members = _userManager.Users.Where(u => u.Role == "Member").ToList();
            return View(members);
        }
        public IActionResult AddMember()
        {
            return View();
        }

        // -------------------- ADD MEMBER --------------------
        [HttpPost]
        public async Task<IActionResult> AddMember(ApplicationUser model, string Password, string ConfirmPassword)
        {
            if (Password != ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                return View("Index", model);
            }

            // Remove Role from validation since we're hardcoding it
            ModelState.Remove("Role");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    HeightFeet = model.HeightFeet,
                    HeightInches = model.HeightInches,
                    Weight = model.Weight,
                    JoinDate = model.JoinDate,          // Added field
                    FitnessGoal = model.FitnessGoal,    // Added field
                    Role = "Member" // Hardcoded role
                };

                var result = await _userManager.CreateAsync(user, Password);

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Member created successfully!";
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }

            return View("AddMember", model);
        }

        // -------------------- EDIT MEMBER (GET) --------------------
        [HttpGet]
        public async Task<IActionResult> EditMember(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // -------------------- EDIT MEMBER (POST) --------------------
        [HttpPost]
        public async Task<IActionResult> EditMember(ApplicationUser model, string NewPassword, string ConfirmPassword)
        {
            ModelState.Remove("PasswordHash");
            ModelState.Remove("Password");
            ModelState.Remove("NewPassword");
            ModelState.Remove("ConfirmPassword");
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return NotFound();

            // Update general info
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.HeightFeet = model.HeightFeet;
            user.HeightInches = model.HeightInches;
            user.Weight = model.Weight;
            user.JoinDate = model.JoinDate;          
            user.FitnessGoal = model.FitnessGoal;    

            // Update user details
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }

            // Handle password change (optional)
            if (!string.IsNullOrWhiteSpace(NewPassword))
            {
                if (NewPassword != ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                    return View(model);
                }

                // Remove old password and add new one
                var removePasswordResult = await _userManager.RemovePasswordAsync(user);
                if (!removePasswordResult.Succeeded)
                {
                    foreach (var error in removePasswordResult.Errors)
                        ModelState.AddModelError("", error.Description);
                    return View(model);
                }

                var addPasswordResult = await _userManager.AddPasswordAsync(user, NewPassword);
                if (!addPasswordResult.Succeeded)
                {
                    foreach (var error in addPasswordResult.Errors)
                        ModelState.AddModelError("", error.Description);
                    return View(model);
                }
            }

            TempData["SuccessMessage"] = "Member updated successfully!";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> DeleteMember(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
                TempData["SuccessMessage"] = "Member deleted successfully!";
            else
                TempData["SuccessMessage"] = "Error deleting member.";

            return RedirectToAction("Index");
        }
    }
}