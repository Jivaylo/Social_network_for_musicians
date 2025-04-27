using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Data;
using SocialNetworkMusician.Models;

namespace SocialNetworkMusician.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Create(Guid? trackId, string? reportedUserId)
        {
            var model = new ReportViewModel
            {
                TrackId = trackId,
                ReportedUserId = reportedUserId
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReportViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);

            var report = new Report
            {
                Id = Guid.NewGuid(),
                ReporterId = user.Id,
                ReportedUserId = model.ReportedUserId,
                TrackId = model.TrackId,
                Reason = model.Reason,
                ReportedAt = DateTime.UtcNow
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            TempData["Success"] = "✅ Report submitted successfully.";
            return RedirectToAction("Index", "Tracks");
        }


        [HttpGet]
        public IActionResult ReportTrack(Guid trackId)
        {
            return View(new ReportViewModel { TrackId = trackId });
        }

        [HttpPost]
        public async Task<IActionResult> ReportTrack(ReportViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid) return View(model);

            var report = new Report
            {
                Id = Guid.NewGuid(),
                ReporterId = user.Id,
                TrackId = model.TrackId,
                Reason = model.Reason
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Thank you for your report. We'll review it soon.";
            return RedirectToAction("Index", "Tracks");
        }

        [HttpGet]
        public IActionResult ReportUser(string userId)
        {
            return View(new ReportViewModel { ReportedUserId = userId });
        }

        [HttpPost]
        public async Task<IActionResult> ReportUser(ReportViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid) return View(model);

            var report = new Report
            {
                Id = Guid.NewGuid(),
                ReporterId = user.Id,
                ReportedUserId = model.ReportedUserId,
                Reason = model.Reason
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            TempData["Success"] = "User report submitted.";
            return RedirectToAction("Index", "Users");
        }
    }
}
