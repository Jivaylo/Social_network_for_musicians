using SocialNetworkMusician.Models;

namespace SocialNetworkMusician.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<AdminUserViewModel>> GetAdminDashboardAsync(string sortBy, string direction);
        Task PromoteToAdminAsync(string userId);
        Task BanUserAsync(string userId);
        Task UnbanUserAsync(string userId);
        Task<List<ReportViewModel>> GetReportsAsync();
    }
}
