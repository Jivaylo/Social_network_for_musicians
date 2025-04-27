using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Data;
using SocialNetworkMusician.Models;
using SocialNetworkMusician.Services.Interfaces;

namespace SocialNetworkMusician.Services.Implementations
{
    public class ReportsService : IReportsService
    {
        private readonly ApplicationDbContext _context;

        public ReportsService(ApplicationDbContext context)
        {
            _context = context;
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
    }
}
