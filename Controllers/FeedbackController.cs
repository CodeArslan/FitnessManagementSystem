using FitnessManagementSystem.DTOs.FeedbackDTOs;
using FitnessManagementSystem.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessManagementSystem.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService _feedbackService;
        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }
        public IActionResult Index()
        {
            return View();
        }

        // GET: Feedback/Create?trainerId=U2&sessionId=S123
        public IActionResult Create(string trainerId, string? sessionId)
        {
            ViewBag.TrainerId = trainerId;
            ViewBag.SessionId = sessionId;

            var dto = new FeedbackCreateDto(
                MemberId: User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)!,
                TrainerId: trainerId,
                Rating: 5, // default
                Comments: null,
                SessionId: sessionId
            );

            return View(dto);
        }

        // POST: Ratings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FeedbackCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TrainerId = dto.TrainerId;
                return View(dto);
            }

            // Call service to submit rating
            await _feedbackService.SubmitFeedbackAsync(dto);

            TempData["Message"] = "Thank you! Your rating has been submitted.";
            return RedirectToAction("TrainerDashboard", new { trainerId = dto.TrainerId });
        }
        // GET: Ratings/TrainerDashboard?trainerId=U2
        public async Task<IActionResult> TrainerDashboard(string trainerId)
        {
            var ratings = await _feedbackService.GetTrainerFeedbackAsync(trainerId);
            //var trainerProfile = await _feedbackService.GetTrainerProfileAsync(trainerId);

            //ViewBag.TrainerProfile = trainerProfile;
            return View(ratings);
        }

    }
}
