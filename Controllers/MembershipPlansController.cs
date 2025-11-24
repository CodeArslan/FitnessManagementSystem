using FitnessManagementSystem.Data;
using FitnessManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace FitnessManagementSystem.Controllers
{
    [Authorize]
    public class MembershipPlansController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        public MembershipPlansController(ApplicationDbContext context, IConfiguration config,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _config = config;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var plans = _context.MembershipPlans.ToList();
            return View(plans);
        }

        [HttpPost]
        [Authorize]
        public IActionResult BuyPlan(int planId)
        {
            var plan = _context.MembershipPlans.FirstOrDefault(p => p.MembershipPlanId == planId);
            if (plan == null)
                return NotFound();

            // Get current logged-in user Id
            var userId = _userManager.GetUserId(User); 

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(plan.Price * 100),
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = plan.PlanName,
                        Description = plan.Description
                    }
                },
                Quantity = 1
            }
        },
                Mode = "payment",
                // Pass planId and userId to Success URL
                SuccessUrl = Url.Action("Success", "MembershipPlans", new { planId = plan.MembershipPlanId }, Request.Scheme),
                CancelUrl = Url.Action("Index", "MembershipPlans", null, Request.Scheme)
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Redirect(session.Url);
        }
        public async Task<IActionResult> Success(int planId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Index", "MembershipPlans");

            user.PlanId = planId;
            await _userManager.UpdateAsync(user);

            var plan = _context.MembershipPlans.FirstOrDefault(p => p.MembershipPlanId == planId);

            return View(plan);
        }

    }
}
