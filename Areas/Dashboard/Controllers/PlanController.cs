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
    public class PlanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // -------------------- INDEX --------------------
        public async Task<IActionResult> Index()
        {
            var plans = await _context.Plans
                .Include(p => p.Member)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(plans);
        }

        // -------------------- ADD PLAN (GET) --------------------
        [HttpGet]
        public async Task<IActionResult> AddPlan()
        {
            await LoadViewData();
            return View(new Plan());
        }

        // -------------------- ADD PLAN (POST) --------------------
        [HttpPost]
        public async Task<IActionResult> AddPlan(Plan model)
        {
            ModelState.Remove("Member");

            if (ModelState.IsValid)
            {
                var plan = new Plan
                {
                    MemberId = model.MemberId,
                    PlanType = model.PlanType,
                    PlanDetails = model.PlanDetails,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.Plans.Add(plan);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Plan created successfully!";
                return RedirectToAction("Index");
            }

            await LoadViewData();
            return View(model);
        }

        // -------------------- EDIT PLAN (GET) --------------------
        [HttpGet]
        public async Task<IActionResult> EditPlan(int id)
        {
            var plan = await _context.Plans
                .Include(p => p.Member)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plan == null)
                return NotFound();

            var viewModel = new Plan
            {
                Id = plan.Id,
                MemberId = plan.MemberId,
                PlanType = plan.PlanType,
                PlanDetails = plan.PlanDetails,
                StartDate = plan.StartDate,
                EndDate = plan.EndDate
            };

            // Pass timestamp information to view
            ViewBag.CreatedAt = $"{plan.CreatedAt:MM/dd/yyyy hh:mm tt}";
            ViewBag.UpdatedAt = $"{plan.UpdatedAt:MM/dd/yyyy hh:mm tt}";

            await LoadViewData();
            return View(viewModel);
        }

        // -------------------- EDIT PLAN (POST) --------------------
        [HttpPost]
        public async Task<IActionResult> EditPlan(Plan model)
        {
            ModelState.Remove("Member");

            if (ModelState.IsValid)
            {
                var existingPlan = await _context.Plans.FindAsync(model.Id);
                if (existingPlan == null)
                    return NotFound();

                // Store original dates for display
                var originalCreatedAt = existingPlan.CreatedAt;
                var originalUpdatedAt = existingPlan.UpdatedAt;

                existingPlan.MemberId = model.MemberId;
                existingPlan.PlanType = model.PlanType;
                existingPlan.PlanDetails = model.PlanDetails;
                existingPlan.StartDate = model.StartDate;
                existingPlan.EndDate = model.EndDate;
                existingPlan.UpdatedAt = DateTime.Now; // Update the timestamp

                _context.Plans.Update(existingPlan);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Plan updated successfully!";
                return RedirectToAction("Index");
            }

            await LoadViewData();
            return View(model);
        }

        // -------------------- DELETE PLAN --------------------
        [HttpGet]
        public async Task<IActionResult> DeletePlan(int id)
        {
            var plan = await _context.Plans.FindAsync(id);
            if (plan == null)
                return NotFound();

            _context.Plans.Remove(plan);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Plan deleted successfully!";
            return RedirectToAction("Index");
        }

        // -------------------- PRIVATE METHODS --------------------
        private async Task LoadViewData()
        {
            var members = await _context.ApplicationUsers
                .Where(u => u.Role == "Member")
                .Select(m => new { m.Id, FullName = $"{m.FirstName} {m.LastName}" })
                .ToListAsync();

            var planTypes = new List<SelectListItem>
            {
                new SelectListItem { Value = "Workout", Text = "Workout Plan" },
                new SelectListItem { Value = "Diet", Text = "Diet Plan" }
            };

            ViewBag.Members = new SelectList(members, "Id", "FullName");
            ViewBag.PlanTypes = new SelectList(planTypes, "Value", "Text");
        }
    }
}