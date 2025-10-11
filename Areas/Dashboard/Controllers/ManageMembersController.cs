using FitnessManagementSystem.Data;
using FitnessManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FitnessManagementSystem.Controllers
{
    [Area("Dashboard")]
    public class ManageMembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManageMembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ManageMembers
        public async Task<IActionResult> Index()
        {
            var members = await _context.ApplicationUsers.ToListAsync();
            return View(members);
        }

        // GET: ManageMembers/Add
        public IActionResult Add()
        {
            return View();
        }

        // POST: ManageMembers/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ApplicationUser member)
        {
            if (ModelState.IsValid)
            {
                member.UserName = member.Email;
                _context.ApplicationUsers.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: ManageMembers/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            var member = await _context.ApplicationUsers.FindAsync(id);
            if (member == null) return NotFound();
            return View(member);
        }

        // POST: ManageMembers/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser member)
        {
            if (id != member.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ApplicationUsers.Any(e => e.Id == member.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: ManageMembers/Details/{id}
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var member = await _context.ApplicationUsers.FirstOrDefaultAsync(m => m.Id == id);
            if (member == null) return NotFound();

            return View(member);
        }

        // POST: ManageMembers/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var member = await _context.ApplicationUsers.FindAsync(id);
            if (member != null)
            {
                _context.ApplicationUsers.Remove(member);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
