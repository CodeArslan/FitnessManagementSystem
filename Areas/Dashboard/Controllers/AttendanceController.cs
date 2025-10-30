using FitnessManagementSystem.Models;
using FitnessManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace FitnessManagementSystem.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AttendanceController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // -------------------- INDEX --------------------
        public async Task<IActionResult> Index(DateTime? attendanceDate)
        {
            try
            {
                var attendances = _context.Attendances
                    .Include(a => a.Member)
                    .Include(a => a.Trainer)
                    .AsQueryable();

                if (attendanceDate.HasValue)
                {
                    attendances = attendances.Where(a => a.AttendanceDate == attendanceDate.Value.Date);
                }

                var attendanceList = await attendances
                    .OrderByDescending(a => a.AttendanceDate)
                    .ThenBy(a => a.Member.FirstName)
                    .ToListAsync();

                ViewBag.AttendanceDate = attendanceDate?.ToString("yyyy-MM-dd");
                return View(attendanceList);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Invalid object name"))
                {
                    ViewBag.Error = "Attendance table is not ready yet.";
                    return View(new List<Attendance>());
                }
                throw;
            }
        }

        // -------------------- ADD ATTENDANCE (GET) --------------------
        [HttpGet]
        public async Task<IActionResult> AddAttendance()
        {
            try
            {
                var members = await _context.ApplicationUsers
                    .Where(u => u.Role == "Member")
                    .OrderBy(m => m.FirstName)
                    .ThenBy(m => m.LastName)
                    .ToListAsync();

                ViewBag.Members = members;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Members = new List<ApplicationUser>();
                ViewBag.Error = "Error loading members: " + ex.Message;
                return View();
            }
        }

        // -------------------- ADD ATTENDANCE (POST) --------------------
        [HttpPost]
        public async Task<IActionResult> AddAttendance(DateTime AttendanceDate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentUser = await _userManager.GetUserAsync(User);
                    if (currentUser == null)
                    {
                        TempData["ErrorMessage"] = "Please log in to mark attendance.";
                        return RedirectToAction("Login", "Account", new { area = "Identity" });
                    }

                    var form = await HttpContext.Request.ReadFormAsync();
                    var memberIds = form["MemberIds"].ToList();

                    if (memberIds.Count == 0)
                    {
                        TempData["ErrorMessage"] = "Please select at least one member.";
                        await LoadMembers();
                        return View();
                    }

                    int recordsAdded = 0;

                    foreach (var memberId in memberIds)
                    {
                        var statusKey = $"Status_{memberId}";
                        var status = form[statusKey].ToString();

                        if (string.IsNullOrEmpty(status))
                        {
                            status = "Present";
                        }

                        var existingAttendance = await _context.Attendances
                            .FirstOrDefaultAsync(a => a.MemberId == memberId && a.AttendanceDate == AttendanceDate.Date);

                        if (existingAttendance == null)
                        {
                            var attendance = new Attendance
                            {
                                MemberId = memberId,
                                TrainerId = currentUser.Id,
                                AttendanceDate = AttendanceDate.Date,
                                Status = status,
                                MarkedAt = DateTime.Now
                            };

                            _context.Attendances.Add(attendance);
                            recordsAdded++;
                        }
                    }

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Attendance marked for {recordsAdded} member(s) successfully!";
                    return RedirectToAction("Index");
                }

                await LoadMembers();
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error marking attendance: " + ex.Message;
                await LoadMembers();
                return View();
            }
        }

        // -------------------- EDIT ATTENDANCE (GET) - Uses DateTime --------------------
        [HttpGet]
        public async Task<IActionResult> EditAttendance(DateTime date)
        {
            try
            {
                var attendances = await _context.Attendances
                    .Include(a => a.Member)
                    .Include(a => a.Trainer)
                    .Where(a => a.AttendanceDate == date.Date)
                    .ToListAsync();

                if (!attendances.Any())
                {
                    TempData["ErrorMessage"] = "No attendance records found for the selected date.";
                    return RedirectToAction("Index");
                }

                ViewBag.AttendanceDate = date;
                return View(attendances);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading attendance: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // -------------------- EDIT ATTENDANCE (POST) - Uses EditAttendanceModel --------------------
        [HttpPost]
        public async Task<IActionResult> EditAttendance(EditAttendanceModel model)
        {
            try
            {
                var form = await HttpContext.Request.ReadFormAsync();
                int recordsUpdated = 0;

                foreach (var key in form.Keys)
                {
                    if (key.StartsWith("Status_"))
                    {
                        var attendanceIdStr = key.Replace("Status_", "");
                        if (int.TryParse(attendanceIdStr, out int attendanceId))
                        {
                            var status = form[key].ToString();

                            var attendance = await _context.Attendances.FindAsync(attendanceId);
                            if (attendance != null)
                            {
                                attendance.Status = status;
                                attendance.MarkedAt = DateTime.Now;
                                recordsUpdated++;
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Attendance updated for {recordsUpdated} member(s) successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error updating attendance: " + ex.Message;
                return RedirectToAction("EditAttendance", new { date = model.AttendanceDate });
            }
        }

        // -------------------- DELETE ATTENDANCE BY DATE --------------------
        [HttpGet]
        public async Task<IActionResult> DeleteAttendanceByDate(DateTime date)
        {
            try
            {
                var attendances = await _context.Attendances
                    .Where(a => a.AttendanceDate == date.Date)
                    .ToListAsync();

                if (!attendances.Any())
                {
                    TempData["ErrorMessage"] = "No attendance records found for the selected date.";
                    return RedirectToAction("Index");
                }

                _context.Attendances.RemoveRange(attendances);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"All attendance records for {date.ToString("MM/dd/yyyy")} deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting attendance: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // -------------------- PRIVATE METHODS --------------------
        private async Task LoadMembers()
        {
            try
            {
                var members = await _context.ApplicationUsers
                    .Where(u => u.Role == "Member")
                    .OrderBy(m => m.FirstName)
                    .ThenBy(m => m.LastName)
                    .ToListAsync();

                ViewBag.Members = members;
            }
            catch (Exception)
            {
                ViewBag.Members = new List<ApplicationUser>();
            }
        }
    }

    // Simple Model for POST method
    public class EditAttendanceModel
    {
        public DateTime AttendanceDate { get; set; }
    }
}