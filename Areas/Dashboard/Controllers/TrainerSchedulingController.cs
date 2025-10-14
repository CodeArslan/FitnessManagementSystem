using FitnessManagementSystem.Models;
using FitnessManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FitnessManagementSystem.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class TrainerSchedulingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainerSchedulingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // -------------------- INDEX --------------------
        public async Task<IActionResult> Index()
        {
            var trainerShifts = await _context.TrainerShifts
                .Include(ts => ts.Trainer)
                .Include(ts => ts.Shift)
                .OrderBy(ts => ts.Date)
                .ThenBy(ts => ts.Shift.StartTime)
                .ToListAsync();

            return View(trainerShifts);
        }

        // -------------------- ADD TRAINER SCHEDULE (GET) --------------------
        [HttpGet]
        public async Task<IActionResult> AddTrainerSchedule()
        {
            await LoadViewData();
            return View();
        }

        // -------------------- ADD TRAINER SCHEDULE (POST) --------------------
        [HttpPost]
        public async Task<IActionResult> AddTrainerSchedule(DateTime Date, int ShiftId, List<string> TrainerIds)
        {
            if (ModelState.IsValid && TrainerIds != null && TrainerIds.Any())
            {
                foreach (var trainerId in TrainerIds)
                {
                    // Check if this trainer is already assigned to this shift on this date
                    var existingAssignment = await _context.TrainerShifts
                        .FirstOrDefaultAsync(ts => ts.Date == Date && ts.ShiftId == ShiftId && ts.TrainerId == trainerId);

                    if (existingAssignment == null)
                    {
                        var trainerSchedule = new TrainerShift
                        {
                            Date = Date,
                            ShiftId = ShiftId,
                            TrainerId = trainerId
                        };

                        _context.TrainerShifts.Add(trainerSchedule);
                    }
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Schedule assigned to {TrainerIds.Count} trainer(s) successfully!";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Please select at least one trainer.");
            await LoadViewData();
            return View();
        }

        // -------------------- ADD SHIFT (Modal - GET) --------------------
        [HttpGet]
        public IActionResult AddShift()
        {
            return PartialView("_AddShiftPartial");
        }

        // -------------------- ADD SHIFT (Modal - POST) --------------------
        [HttpPost]
        public async Task<IActionResult> AddShift(Shift model)
        {
            if (ModelState.IsValid)
            {
                _context.Shifts.Add(model);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Shift added successfully!" });
            }

            return Json(new { success = false, message = "Please check the form for errors." });
        }

        // -------------------- EDIT TRAINER SCHEDULE (GET) --------------------
        [HttpGet]
        public async Task<IActionResult> EditTrainerSchedule(DateTime date, int shiftId)
        {
            // Get all trainer assignments for this specific date and shift
            var trainerShifts = await _context.TrainerShifts
                .Include(ts => ts.Trainer)
                .Include(ts => ts.Shift)
                .Where(ts => ts.Date == date && ts.ShiftId == shiftId)
                .ToListAsync();

            if (!trainerShifts.Any())
                return NotFound();

            // Get selected trainer IDs
            var selectedTrainerIds = trainerShifts.Select(ts => ts.TrainerId).ToList();

            await LoadViewData();
            ViewBag.Date = date;
            ViewBag.ShiftId = shiftId;
            ViewBag.SelectedTrainerIds = selectedTrainerIds;
            ViewBag.TrainerShifts = trainerShifts;

            return View();
        }

        // -------------------- EDIT TRAINER SCHEDULE (POST) --------------------
        [HttpPost]
        public async Task<IActionResult> EditTrainerSchedule(DateTime Date, int ShiftId, List<string> TrainerIds)
        {
            if (ModelState.IsValid && TrainerIds != null && TrainerIds.Any())
            {
                // Get existing assignments for this date and shift
                var existingAssignments = await _context.TrainerShifts
                    .Where(ts => ts.Date == Date && ts.ShiftId == ShiftId)
                    .ToListAsync();

                // Remove assignments that are no longer selected
                var assignmentsToRemove = existingAssignments
                    .Where(ea => !TrainerIds.Contains(ea.TrainerId))
                    .ToList();

                _context.TrainerShifts.RemoveRange(assignmentsToRemove);

                // Add new assignments that weren't previously selected
                var existingTrainerIds = existingAssignments.Select(ea => ea.TrainerId).ToList();
                var trainersToAdd = TrainerIds
                    .Where(trainerId => !existingTrainerIds.Contains(trainerId))
                    .ToList();

                foreach (var trainerId in trainersToAdd)
                {
                    var trainerSchedule = new TrainerShift
                    {
                        Date = Date,
                        ShiftId = ShiftId,
                        TrainerId = trainerId
                    };
                    _context.TrainerShifts.Add(trainerSchedule);
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Schedule updated successfully! {trainersToAdd.Count} trainer(s) added, {assignmentsToRemove.Count} trainer(s) removed.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Please select at least one trainer.");
            await LoadViewData();
            ViewBag.SelectedTrainerIds = TrainerIds ?? new List<string>();
            return View();
        }

        // -------------------- DELETE TRAINER SCHEDULE --------------------
        [HttpGet]
        public async Task<IActionResult> DeleteTrainerSchedule(int id)
        {
            var trainerShift = await _context.TrainerShifts.FindAsync(id);
            if (trainerShift == null)
                return NotFound();

            _context.TrainerShifts.Remove(trainerShift);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Trainer schedule deleted successfully!";
            return RedirectToAction("Index");
        }

        // -------------------- DELETE SCHEDULE (All trainers for a shift) --------------------
        [HttpGet]
        public async Task<IActionResult> DeleteSchedule(DateTime date, int shiftId)
        {
            var trainerShifts = await _context.TrainerShifts
                .Where(ts => ts.Date == date && ts.ShiftId == shiftId)
                .ToListAsync();

            if (!trainerShifts.Any())
                return NotFound();

            _context.TrainerShifts.RemoveRange(trainerShifts);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Schedule deleted successfully! Removed {trainerShifts.Count} trainer assignment(s).";
            return RedirectToAction("Index");
        }

        // -------------------- GET SHIFTS FOR DROPDOWN --------------------
        [HttpGet]
        public async Task<IActionResult> GetShifts()
        {
            var shifts = await _context.Shifts
                .Select(s => new { s.ShiftId, s.Name })
                .ToListAsync();

            return Json(shifts);
        }

        // -------------------- GET TRAINERS FOR DROPDOWN --------------------
        [HttpGet]
        public async Task<IActionResult> GetTrainers()
        {
            var trainers = await _context.ApplicationUsers
                .Where(u => u.Role == "Trainer")
                .Select(t => new { t.Id, FullName = $"{t.FirstName} {t.LastName}" })
                .ToListAsync();

            return Json(trainers);
        }

        // -------------------- PRIVATE METHODS --------------------
        private async Task LoadViewData()
        {
            var trainers = await _context.ApplicationUsers
                .Where(u => u.Role == "Trainer")
                .Select(t => new { t.Id, FullName = $"{t.FirstName} {t.LastName}" })
                .ToListAsync();

            var shifts = await _context.Shifts.ToListAsync();

            ViewBag.Trainers = new MultiSelectList(trainers, "Id", "FullName");
            ViewBag.Shifts = new SelectList(shifts, "ShiftId", "Name");
        }
    }
}