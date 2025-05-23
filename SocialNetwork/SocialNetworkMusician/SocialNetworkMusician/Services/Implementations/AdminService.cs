using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Data;
using SocialNetworkMusician.Models;
using SocialNetworkMusician.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SocialNetworkMusician.Services.Implementations
{
    // Дефиниране на AdminService, който имплементира IAdminService
    public class AdminService : IAdminService
    {
        // Променлива за достъп до базата данни
        private readonly ApplicationDbContext _context;

        // Променлива за работа с потребители
        private readonly UserManager<ApplicationUser> _userManager;

        // Променлива за изпращане на имейли
        private readonly IEmailSender _emailSender;

        // Конструктор за инжектиране на зависимостите
        public AdminService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _context = context; // Инициализиране на контекста
            _userManager = userManager; // Инициализиране на мениджъра на потребители
            _emailSender = emailSender; // Инициализиране на изпращача на имейли
        }

        // Метод за получаване на потребителите за админ таблото
        public async Task<List<AdminUserViewModel>> GetAdminDashboardAsync(string sortBy, string direction)
        {
            // Извличане на всички потребители от базата
            var users = await _userManager.Users.ToListAsync();

            // Извличане на всички песни
            var tracks = await _context.MusicTracks.ToListAsync();

            // Създаване на списък с изгледи за потребителите
            var viewModels = new List<AdminUserViewModel>();

            // Цикъл през всеки потребител
            foreach (var user in users)
            {
                // Проверка дали потребителят е админ
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

                // Проверка дали потребителят е модератор
                var isModerator = await _userManager.IsInRoleAsync(user, "Moderator");

                // Добавяне на потребителя към изгледа
                viewModels.Add(new AdminUserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    IsAdmin = isAdmin,
                    IsModerator = isModerator,
                    IsBanned = user.LockoutEnd != null && user.LockoutEnd > DateTime.UtcNow, // Проверка дали е баннат
                    JoinedDate = user.JoinedDate
                });
            }

            // Сортиране на изгледите според подадените параметри
            viewModels = (sortBy, direction) switch
            {
                ("email", "asc") => viewModels.OrderBy(u => u.Email).ToList(),
                ("email", "desc") => viewModels.OrderByDescending(u => u.Email).ToList(),
                ("display", "asc") => viewModels.OrderBy(u => u.DisplayName).ToList(),
                ("display", "desc") => viewModels.OrderByDescending(u => u.DisplayName).ToList(),
                ("role", _) => viewModels
                    .OrderByDescending(u => u.IsAdmin)
                    .ThenByDescending(u => u.IsModerator)
                    .ToList(),
                _ => viewModels // По подразбиране не сортирай
            };

            // Връщане на резултата
            return viewModels;
        }


        // Метод за банване на потребител
        public async Task BanUserAsync(string userId)
        {
            // Намиране на потребителя
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                // Активиране на блокировката и задаване на дълъг период
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTime.UtcNow.AddYears(100);
                await _userManager.UpdateAsync(user);

                // Изпращане на имейл за бан
                await _emailSender.SendEmailAsync(
                    user.Email,
                    "⚠️ Account Banned",
                    $"Hi {user.DisplayName},<br><br>Your account has been <strong>banned</strong> on SoundSocial."
                );
            }
        }

        // Метод за махане на бан от потребител
        public async Task UnbanUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                // Премахване на LockoutEnd
                user.LockoutEnd = null;
                await _userManager.UpdateAsync(user);

                // Изпращане на имейл за разблокиране
                await _emailSender.SendEmailAsync(
                    user.Email,
                    "✅ Your Account Is Active Again",
                    $"Hi {user.DisplayName},<br><br>Your account on SoundSocial has been <strong>unbanned</strong>."
                );
            }
        }

        // Метод за вземане на всички репорти
        public async Task<List<ReportViewModel>> GetReportsAsync()
        {
            var reports = await _context.Reports
                .Include(r => r.Reporter) // Включване на докладващия
                .Include(r => r.ReportedUser) // Включване на докладвания
                .Include(r => r.Track) // Включване на песента
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
                .ToListAsync(); // Извличане на списък

            return reports;
        }

        // Метод за промотиране на потребител в модератор
        public async Task PromoteToModeratorAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null && !await _userManager.IsInRoleAsync(user, "Moderator"))
            {
                await _userManager.AddToRoleAsync(user, "Moderator");

                await _emailSender.SendEmailAsync(
                    user.Email,
                    "🎤 You're a Moderator Now!",
                    $"Hi {user.DisplayName},<br><br>You've been promoted to <strong>Moderator</strong> on <strong>SoundSocial</strong>!"
                );
            }
        }


        // Метод за премахване на модераторски права
        public async Task UnpromoteFromModeratorAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null && await _userManager.IsInRoleAsync(user, "Moderator"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Moderator");

                await _emailSender.SendEmailAsync(
                    user.Email,
                    "🔽 Removed from Moderator",
                    $"Hi {user.DisplayName},<br><br>Your Moderator privileges on <strong>SoundSocial</strong> have been removed."
                );
            }
        }
    }
}
