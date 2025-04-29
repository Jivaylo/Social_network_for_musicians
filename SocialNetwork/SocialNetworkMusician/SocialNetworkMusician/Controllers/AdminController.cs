using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetworkMusician.Data;
using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Models;
using SocialNetworkMusician.Services.Interfaces;
using static SocialNetworkMusician.Data.Data.MusicTrack;

namespace SocialNetworkMusician.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> Index(string sortBy = "display", string direction = "asc")
        {
            var viewModels = await _adminService.GetAdminDashboardAsync(sortBy, direction);
            return View(viewModels);
        }

        [HttpPost]
        public async Task<IActionResult> PromoteToAdmin(string userId)
        {
            await _adminService.PromoteToAdminAsync(userId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> BanUser(string userId)
        {
            await _adminService.BanUserAsync(userId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UnbanUser(string userId)
        {
            await _adminService.UnbanUserAsync(userId);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reports()
        {
            var reports = await _adminService.GetReportsAsync();
            return View(reports);
        }
        [HttpPost]
        public async Task<IActionResult> PromoteToModerator(string userId)
        {
            await _adminService.PromoteToModeratorAsync(userId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UnpromoteFromAdmin(string userId)
        {
            await _adminService.UnpromoteFromAdminAsync(userId);
            return RedirectToAction("Index");
        }
    }
}

