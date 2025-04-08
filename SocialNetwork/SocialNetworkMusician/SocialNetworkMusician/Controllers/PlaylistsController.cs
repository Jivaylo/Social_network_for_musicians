using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Data;
using SocialNetworkMusician.Models;
using Microsoft.EntityFrameworkCore;

namespace SocialNetworkMusician.Controllers
{
    [Authorize]
    public class PlaylistsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PlaylistsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Details(Guid id, string? search)
        {
            var playlist = await _context.Playlists
                .Include(p => p.PlaylistTracks)
                    .ThenInclude(pt => pt.MusicTrack)
                .ThenInclude(mt => mt.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (playlist == null)
                return NotFound();

            var allTrackIdsInPlaylist = playlist.PlaylistTracks.Select(pt => pt.MusicTrackId).ToList();

            // Search logic
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
                Tracks = playlist.PlaylistTracks
                    .Select(pt => new TrackViewModel
                    {
                        Id = pt.MusicTrack.Id,
                        Title = pt.MusicTrack.Title,
                        Description = pt.MusicTrack.Description,
                        CategoryName = pt.MusicTrack.Category?.Name,
                        FileUrl = pt.MusicTrack.FileUrl
                    }).ToList()
            };

            ViewBag.AvailableTracks = availableTracks;
            ViewBag.Search = search;

            return View(model);
        }



        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var playlists = await _context.Playlists
                .Include(p => p.PlaylistTracks)
                .ThenInclude(pt => pt.MusicTrack)
                .Where(p => p.UserId == user.Id)
                .ToListAsync();

            var models = playlists.Select(p => new PlaylistViewModel
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

            return View(models);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            var user = await _userManager.GetUserAsync(User);
            var playlist = new Playlist { Id = Guid.NewGuid(), Name = name, UserId = user.Id };
            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddTrack(Guid playlistId, Guid trackId)
        {
            var playlist = await _context.Playlists
                .Include(p => p.PlaylistTracks)
                .FirstOrDefaultAsync(p => p.Id == playlistId);

            if (playlist == null) return NotFound();

            
            if (!playlist.PlaylistTracks.Any(pt => pt.MusicTrackId == trackId))
            {
                playlist.PlaylistTracks.Add(new PlaylistTrack
                {
                    PlaylistId = playlistId,
                    MusicTrackId = trackId
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = playlistId });
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveTrack(Guid playlistId, Guid trackId)
        {
            var playlist = await _context.Playlists
                .Include(p => p.PlaylistTracks)
                .FirstOrDefaultAsync(p => p.Id == playlistId);

            if (playlist == null)
                return NotFound();

            var playlistTrack = playlist.PlaylistTracks.FirstOrDefault(pt => pt.MusicTrackId == trackId);
            if (playlistTrack != null)
            {
                _context.Remove(playlistTrack);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = playlistId });
        }

    }
}
