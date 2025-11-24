using FitnessManagementSystem.Data;
using FitnessManagementSystem.Models;
using FitnessManagementSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FitnessManagementSystem.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class TrainerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _Context;

        public TrainerController(UserManager<ApplicationUser> userManager,ApplicationDbContext context)
        {
            _userManager = userManager;
            _Context = context;
        }
        // -------------------- Dashboard (List Trainers) --------------------
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            // Fetch upcoming appointments
            var appointments = await _Context.Appointments
                .Include(a => a.User)
                .Where(a => a.TrainerId == user.Id && a.Status == "Pending")
                .OrderBy(a => a.SessionDate)
                .ToListAsync();

            return View(appointments);
        }
        // -------------------- INDEX (List Trainers) --------------------
        public async Task<IActionResult> Index()
        {
            var trainers = _userManager.Users.Where(u => u.Role == "Trainer").ToList();
            return View(trainers);
        }

        // -------------------- ADD TRAINER (GET) --------------------
        public IActionResult AddTrainer()
        {
            return View();
        }

        // -------------------- ADD TRAINER (POST) --------------------
        [HttpPost]
        public async Task<IActionResult> AddTrainer(ApplicationUser model, string Password, string ConfirmPassword)
        {
            if (Password != ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                return View("AddTrainer", model);
            }

            // Remove Role from validation since we're hardcoding it
            ModelState.Remove("Role");
            ModelState.Remove("MembershipPlans");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Specialization = model.Specialization,
                    Certification = model.Certification,
                    HireDate = model.HireDate,
                    Role = "Trainer" // Hardcoded as Trainer role
                };

                var result = await _userManager.CreateAsync(user, Password);

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Trainer created successfully!";
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }

            return View("AddTrainer", model);
        }

        // -------------------- EDIT TRAINER (GET) --------------------
        [HttpGet]
        public async Task<IActionResult> EditTrainer(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // -------------------- EDIT TRAINER (POST) --------------------
        [HttpPost]
        public async Task<IActionResult> EditTrainer(ApplicationUser model, string NewPassword, string ConfirmPassword)
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
            user.Specialization = model.Specialization;
            user.Certification = model.Certification;
            user.HireDate = model.HireDate;

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

            TempData["SuccessMessage"] = "Trainer updated successfully!";
            return RedirectToAction("Index");
        }

        // -------------------- DELETE TRAINER --------------------
        [HttpGet]
        public async Task<IActionResult> DeleteTrainer(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
                TempData["SuccessMessage"] = "Trainer deleted successfully!";
            else
                TempData["ErrorMessage"] = "Error deleting trainer.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> MarkCompleted(int appointmentId)
        {
            var appointment = await _Context.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (appointment == null)
            {
                TempData["Error"] = "Appointment not found.";
                return RedirectToAction("Dashboard"); 
            }

            appointment.Status = "Completed";

            _Context.Appointments.Update(appointment);
            await _Context.SaveChangesAsync();

            TempData["Success"] = "Appointment marked as completed.";
            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        public async Task<IActionResult> ManageAvailability()
        {
            var trainer = await _userManager.GetUserAsync(User);
            if (trainer == null) return Challenge();

            var availabilities = await _Context.TrainerAvailabilitys
                .Where(a => a.TrainerId == trainer.Id)
                .OrderBy(a => a.DayOfWeek)
                .ThenBy(a => a.StartTime)
                .ToListAsync();

            return View(availabilities);
        }

        [HttpPost]
        public async Task<IActionResult> AddAvailability(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
        {
            var trainer = await _userManager.GetUserAsync(User);
            if (trainer == null) return Challenge();

            var availability = new TrainerAvailability
            {
                TrainerId = trainer.Id,
                DayOfWeek = dayOfWeek,
                StartTime = startTime,
                EndTime = endTime
            };

            _Context.TrainerAvailabilitys.Add(availability);
            await _Context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageAvailability));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAvailability(int id)
        {
            var availability = await _Context.TrainerAvailabilitys.FindAsync(id);
            if (availability != null)
            {
                var trainer = await _userManager.GetUserAsync(User);
                if (trainer != null && availability.TrainerId == trainer.Id)
                {
                    _Context.TrainerAvailabilitys.Remove(availability);
                    await _Context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(ManageAvailability));
        }
        [HttpGet]
        public async Task<IActionResult> ViewMemberProgress()
        {
            // Get the latest workout session to extract the UserId
            var lastSession = await _Context.WorkoutSessions
                .OrderByDescending(w => w.SessionDate)
                .FirstOrDefaultAsync();

            if (lastSession == null)
            {
                ViewBag.MemberName = "Member";
                return View(new MemberProgressViewModel());
            }

            string memberId = lastSession.UserId; 

            var member = await _userManager.FindByIdAsync(memberId);

            var progressRecords = await _Context.ProgressRecords
                .Where(r => r.UserId == memberId)
                .OrderByDescending(r => r.RecordedAt)
                .ToListAsync();

            var workoutSessions = await _Context.WorkoutSessions
                .Where(w => w.UserId == memberId)
                .OrderByDescending(w => w.SessionDate)
                .ToListAsync();

            ViewBag.MemberName = $"{member?.FirstName} {member?.LastName}".Trim();

            var viewModel = new MemberProgressViewModel
            {
                ProgressRecords = progressRecords,
                WorkoutSessions = workoutSessions
            };

            return View(viewModel);
        }
    }
}