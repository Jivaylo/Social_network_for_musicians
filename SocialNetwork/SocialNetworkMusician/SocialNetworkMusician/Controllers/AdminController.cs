using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetworkMusician.Data;
using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Models;
using static SocialNetworkMusician.Data.Data.MusicTrack;

namespace SocialNetworkMusician.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Index(string sortBy = "email")
        {
            var users = await _userManager.Users.ToListAsync();
            var tracks = await _context.MusicTracks.ToListAsync();

            var viewModels = new List<AdminUserViewModel>();

            foreach (var user in users)
            {
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

                viewModels.Add(new AdminUserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    IsAdmin = isAdmin,
                    IsBanned = user.LockoutEnd != null && user.LockoutEnd > DateTime.UtcNow,
                    JoinedDate = user.JoinedDate 
                });
            }

            
            viewModels = sortBy switch
            {
                "role" => viewModels.OrderByDescending(u => u.IsAdmin).ToList(),
                "joined" => viewModels.OrderByDescending(u => u.JoinedDate).ToList(),
                _ => viewModels.OrderBy(u => u.Email).ToList()
            };

            ViewBag.SortBy = sortBy;
            ViewBag.TrackCount = tracks.Count;
            ViewBag.UserCount = users.Count;
            ViewBag.TotalPlays = tracks.Sum(t => t.PlayCount);

            return View(viewModels);
        }

        [HttpPost]
        public async Task<IActionResult> PromoteToAdmin(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && !await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.AddToRoleAsync(user, "Admin");

               
                await _emailSender.SendEmailAsync(
                    user.Email,
                    "🎉 You've been promoted!",
                    $"Hi {user.DisplayName},<br><br>You've just been promoted to <strong>Admin</strong> on <strong>SoundSocial</strong>. Congrats! 🎧<br><br>— The Team"
                );
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> BanUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTime.UtcNow.AddYears(100);
                await _userManager.UpdateAsync(user);

               
                await _emailSender.SendEmailAsync(
                    user.Email,
                    "⚠️ Account Banned",
                    $"Hi {user.DisplayName},<br><br>Your account has been <strong>banned</strong> on SoundSocial. If you believe this is a mistake, please contact support.<br><br>— The Team"
                );
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UnbanUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.LockoutEnd = null;
                await _userManager.UpdateAsync(user);

                await _emailSender.SendEmailAsync(
                    user.Email,
                    "✅ Your Account Is Active Again",
                    $"Hi {user.DisplayName},<br><br>Your account on SoundSocial has been <strong>unbanned</strong>. You can now log in and enjoy the platform!<br><br>— The Team"
                );
            }
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reports()
        {
            var reports = await _context.Reports
                .Include(r => r.Reporter)
                .Include(r => r.ReportedUser)
                .Include(r => r.Track)
                .Select(r => new ReportViewModel
                {
                    Id = r.Id,
                    ReporterEmail = r.Reporter.Email,
                    ReportedUserEmail = r.ReportedUser != null ? r.ReportedUser.Email : null,
                    TrackTitle = r.Track != null ? r.Track.Title : null,
                    Reason = r.Reason,
                    ReportedAt = r.ReportedAt,
                    ReportedUserId = r.ReportedUserId,
                    TrackId = r.TrackId
                })
                .OrderByDescending(r => r.ReportedAt)
                .ToListAsync();

            return View(reports);
        }

    }
}

