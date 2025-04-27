using SocialNetworkMusician.Models;

namespace SocialNetworkMusician.Services.Interfaces
{
    public interface ITracksService
    {
        Task<List<TrackViewModel>> GetAllTracksAsync();
        Task<TrackViewModel?> GetTrackDetailsAsync(Guid id, string? currentUserId);
        Task CreateTrackAsync(TrackViewModel model, string userId);
        Task DeleteTrackAsync(Guid id, string userId, bool isAdmin);
        Task LikeTrackAsync(Guid id, string userId);
        Task DislikeTrackAsync(Guid id, string userId);
        Task AddCommentAsync(Guid id, string comment, string userId);
        Task IncrementPlayCountAsync(Guid id);
        Task<List<TrackViewModel>> GetTrendingTracksAsync();
        Task EditTrackAsync(Guid id, TrackViewModel model, string userId, bool isAdmin);
    }
}
