using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetworkMusician.Data;
using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Models;
using SocialNetworkMusician.Services.Interfaces;

namespace SocialNetworkMusician.Services.Implementations
{
    public class ReportsService : IReportsService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportsService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public ReportViewModel PrepareCreateModel(Guid? trackId, string? reportedUserId)
        {
            return new ReportViewModel
            {
                TrackId = trackId,
                ReportedUserId = reportedUserId
            };
        }

        public ReportViewModel PrepareReportTrackModel(Guid trackId)
        {
            return new ReportViewModel { TrackId = trackId };
        }

        public ReportViewModel PrepareReportUserModel(string userId)
        {
            return new ReportViewModel { ReportedUserId = userId };
        }

        public async Task SubmitTrackReportAsync(ReportViewModel model, string reporterId)
        {
            var report = new Report
            {
                Id = Guid.NewGuid(),
                ReporterId = reporterId,
                TrackId = model.TrackId,
                ReportedUserId = model.ReportedUserId,
                Reason = model.Reason,
                ReportedAt = DateTime.UtcNow
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();
        }

        public async Task SubmitUserReportAsync(ReportViewModel model, string reporterId)
        {
            var report = new Report
            {
                Id = Guid.NewGuid(),
                ReporterId = reporterId,
                ReportedUserId = model.ReportedUserId,
                Reason = model.Reason,
                ReportedAt = DateTime.UtcNow
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();
        }

        // 🔥 EXTRA: for Admin Panel
        public async Task<List<ReportViewModel>> GetAllReportsAsync()
        {
            var reports = await _context.Reports
                .Include(r => r.Reporter)
                .Include(r => r.ReportedUser)
                .Include(r => r.Track)
                .OrderByDescending(r => r.ReportedAt)
                .ToListAsync();

            return reports.Select(r => new ReportViewModel
            {
                Id = r.Id,
                ReporterEmail = r.Reporter.Email,
                ReportedUserEmail = r.ReportedUser?.Email,
                ReportedUserId = r.ReportedUserId,
                TrackTitle = r.Track?.Title,
                TrackId = r.TrackId,
                Reason = r.Reason,
                ReportedAt = r.ReportedAt
            }).ToList();
        }

        public async Task<bool> BanUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.LockoutEnabled = true;
            user.LockoutEnd = DateTime.UtcNow.AddYears(100);
            await _userManager.UpdateAsync(user);
            return true;
        }

        public async Task<bool> DeleteTrackAsync(Guid trackId)
        {
            var track = await _context.MusicTracks.FindAsync(trackId);
            if (track == null) return false;

            _context.MusicTracks.Remove(track);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
