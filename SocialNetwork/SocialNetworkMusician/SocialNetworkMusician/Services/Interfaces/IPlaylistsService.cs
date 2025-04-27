using SocialNetworkMusician.Models;

namespace SocialNetworkMusician.Services.Interfaces
{
    public interface IPlaylistsService
    {
        Task<PlaylistViewModel?> GetPlaylistDetailsAsync(Guid id, string? search);
        Task<List<PlaylistViewModel>> GetUserPlaylistsAsync(string userId);
        Task CreatePlaylistAsync(string name, string userId);
        Task AddTrackToPlaylistAsync(Guid playlistId, Guid trackId);
        Task RemoveTrackFromPlaylistAsync(Guid playlistId, Guid trackId);
    }
}
