using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Data;
using SocialNetworkMusician.Models;
using Microsoft.EntityFrameworkCore;
using SocialNetworkMusician.Services.Interfaces;

namespace SocialNetworkMusician.Services.Implementations
{
    public class PlaylistsService : IPlaylistsService
    {
        private readonly ApplicationDbContext _context;

        public PlaylistsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PlaylistViewModel?> GetPlaylistDetailsAsync(Guid id, string? search)
        {
            var playlist = await _context.Playlists
                .Include(p => p.PlaylistTracks)
                    .ThenInclude(pt => pt.MusicTrack)
                    .ThenInclude(mt => mt.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (playlist == null) return null;

            var allTrackIdsInPlaylist = playlist.PlaylistTracks.Select(pt => pt.MusicTrackId).ToList();

            var query = _context.MusicTracks
                .Include(t => t.Category)
                .Where(t => !allTrackIdsInPlaylist.Contains(t.Id));

            if (!string.IsNullOrWhiteSpace(search))
            {
                string loweredSearch = search.ToLower();
                query = query.Where(t => t.Title.ToLower().Contains(loweredSearch));
            }

            var availableTracks = await query.ToListAsync();

            var model = new PlaylistViewModel
            {
                Id = playlist.Id,
                Name = playlist.Name,
                Tracks = playlist.PlaylistTracks.Select(pt => new TrackViewModel
                {
                    Id = pt.MusicTrack.Id,
                    Title = pt.MusicTrack.Title,
                    Description = pt.MusicTrack.Description,
                    CategoryName = pt.MusicTrack.Category?.Name,
                    FileUrl = pt.MusicTrack.FileUrl
                }).ToList()
            };

            return model;
        }

        public async Task<List<PlaylistViewModel>> GetUserPlaylistsAsync(string userId)
        {
            var playlists = await _context.Playlists
                .Include(p => p.PlaylistTracks)
                    .ThenInclude(pt => pt.MusicTrack)
                .Where(p => p.UserId == userId)
                .ToListAsync();

            return playlists.Select(p => new PlaylistViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Tracks = p.PlaylistTracks.Select(pt => new TrackViewModel
                {
                    Id = pt.MusicTrack.Id,
                    Title = pt.MusicTrack.Title,
                    FileUrl = pt.MusicTrack.FileUrl
                }).ToList()
            }).ToList();
        }

        public async Task CreatePlaylistAsync(string name, string userId)
        {
            var playlist = new Playlist
            {
                Id = Guid.NewGuid(),
                Name = name,
                UserId = userId
            };

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();
        }

        public async Task AddTrackToPlaylistAsync(Guid playlistId, Guid trackId)
        {
            var playlist = await _context.Playlists
                .Include(p => p.PlaylistTracks)
                .FirstOrDefaultAsync(p => p.Id == playlistId);

            if (playlist == null) throw new Exception("Playlist not found");

            if (!playlist.PlaylistTracks.Any(pt => pt.MusicTrackId == trackId))
            {
                playlist.PlaylistTracks.Add(new PlaylistTrack
                {
                    PlaylistId = playlistId,
                    MusicTrackId = trackId
                });

                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveTrackFromPlaylistAsync(Guid playlistId, Guid trackId)
        {
            var playlist = await _context.Playlists
                .Include(p => p.PlaylistTracks)
                .FirstOrDefaultAsync(p => p.Id == playlistId);

            if (playlist == null) throw new Exception("Playlist not found");

            var playlistTrack = playlist.PlaylistTracks.FirstOrDefault(pt => pt.MusicTrackId == trackId);
            if (playlistTrack != null)
            {
                _context.Remove(playlistTrack);
                await _context.SaveChangesAsync();
            }
        }
    }
}
