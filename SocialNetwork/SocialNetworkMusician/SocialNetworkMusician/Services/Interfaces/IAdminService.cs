using SocialNetworkMusician.Models;

namespace SocialNetworkMusician.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<AdminUserViewModel>> GetAdminDashboardAsync(string sortBy, string direction);
        Task BanUserAsync(string userId);
        Task UnbanUserAsync(string userId);
        Task<List<ReportViewModel>> GetReportsAsync();
        Task PromoteToModeratorAsync(string userId);
        Task UnpromoteFromModeratorAsync(string userId);
    }
}
