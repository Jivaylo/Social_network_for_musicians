using SocialNetworkMusician.Models;

namespace SocialNetworkMusician.Services.Interfaces
{
    public interface IReportsService
    {
        ReportViewModel PrepareCreateModel(Guid? trackId, string? reportedUserId);
        ReportViewModel PrepareReportTrackModel(Guid trackId);
        ReportViewModel PrepareReportUserModel(string userId);
        Task SubmitTrackReportAsync(ReportViewModel model, string reporterId);
        Task SubmitUserReportAsync(ReportViewModel model, string reporterId);
    }
}
