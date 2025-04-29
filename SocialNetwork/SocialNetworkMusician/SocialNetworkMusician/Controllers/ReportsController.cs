using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Models;
using SocialNetworkMusician.Services.Interfaces;

namespace SocialNetworkMusician.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly IReportsService _reportsService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportsController(IReportsService reportsService, UserManager<ApplicationUser> userManager)
        {
            _reportsService = reportsService;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Create(Guid? trackId, string? reportedUserId)
        {
            var model = _reportsService.PrepareCreateModel(trackId, reportedUserId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReportViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            await _reportsService.SubmitTrackReportAsync(model, user.Id);

            TempData["Success"] = "✅ Report submitted successfully.";
            return RedirectToAction("Index", "Tracks");
        }

        [HttpGet]
        public IActionResult ReportTrack(Guid trackId)
        {
            var model = _reportsService.PrepareReportTrackModel(trackId);
            return View("Create", model); // same Create form
        }

        [HttpPost]
        public async Task<IActionResult> ReportTrack(ReportViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Create", model);

            var user = await _userManager.GetUserAsync(User);
            await _reportsService.SubmitTrackReportAsync(model, user.Id);

            TempData["Success"] = "✅ Track report submitted.";
            return RedirectToAction("Index", "Tracks");
        }

        [HttpGet]
        public IActionResult ReportUser(string userId)
        {
            var model = _reportsService.PrepareReportUserModel(userId);
            return View("Create", model); 
        }

        [HttpPost]
        public async Task<IActionResult> ReportUser(ReportViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Create", model);

            var user = await _userManager.GetUserAsync(User);
            await _reportsService.SubmitUserReportAsync(model, user.Id);

            TempData["Success"] = "✅ User report submitted.";
            return RedirectToAction("Index", "Users");
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.HideFooter = true;
            var reports = await _reportsService.GetAllReportsAsync();
            return View(reports);
        }
    }
}
