using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Data;
using SocialNetworkMusician.Models;
using SocialNetworkMusician.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SocialNetworkMusician.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public AdminService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<List<AdminUserViewModel>> GetAdminDashboardAsync(string sortBy, string direction)
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

            viewModels = (sortBy, direction) switch
            {
                ("email", "asc") => viewModels.OrderBy(u => u.Email).ToList(),
                ("email", "desc") => viewModels.OrderByDescending(u => u.Email).ToList(),
                ("display", "asc") => viewModels.OrderBy(u => u.DisplayName).ToList(),
                ("display", "desc") => viewModels.OrderByDescending(u => u.DisplayName).ToList(),
                ("role", _) => viewModels.OrderByDescending(u => u.IsAdmin).ToList(),
                _ => viewModels
            };

            return viewModels;
        }

        public async Task PromoteToAdminAsync(string userId)
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
        }

        public async Task BanUserAsync(string userId)
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
                    $"Hi {user.DisplayName},<br><br>Your account has been <strong>banned</strong> on SoundSocial."
                );
            }
        }

        public async Task UnbanUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.LockoutEnd = null;
                await _userManager.UpdateAsync(user);

                await _emailSender.SendEmailAsync(
                    user.Email,
                    "✅ Your Account Is Active Again",
                    $"Hi {user.DisplayName},<br><br>Your account on SoundSocial has been <strong>unbanned</strong>."
                );
            }
        }

        public async Task<List<ReportViewModel>> GetReportsAsync()
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

            return reports;
        }
    }
}
