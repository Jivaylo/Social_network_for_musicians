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
        public async Task<IActionResult> Details(Guid id)
        {
            var playlist = await _context.Playlists
                .Include(p => p.PlaylistTracks)
                .ThenInclude(pt => pt.MusicTrack)
                .ThenInclude(t => t.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (playlist == null)
                return NotFound();

            var viewModel = new PlaylistViewModel
            {
                Id = playlist.Id,
                Name = playlist.Name,
                Tracks = playlist.PlaylistTracks.Select(pt => new TrackViewModel
                {
                    Id = pt.MusicTrack.Id,
                    Title = pt.MusicTrack.Title,
                    Description = pt.MusicTrack.Description,
                    CategoryName = pt.MusicTrack.Category?.Name,
                    UploadedAt = pt.MusicTrack.UploadedAt,
                    FileUrl = pt.MusicTrack.FileUrl,
                    UserName = pt.MusicTrack.User?.UserName
                }).ToList()
            };

            return View(viewModel);
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

        public async Task<IActionResult> AddTrack(Guid playlistId, Guid trackId)
        {
            var exists = await _context.PlaylistTracks
                .AnyAsync(pt => pt.PlaylistId == playlistId && pt.MusicTrackId == trackId);
            if (!exists)
            {
                _context.PlaylistTracks.Add(new PlaylistTrack
                {
                    Id = Guid.NewGuid(),
                    PlaylistId = playlistId,
                    MusicTrackId = trackId
                });
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveTrack(Guid playlistId, Guid trackId)
        {
            var item = await _context.PlaylistTracks
                .FirstOrDefaultAsync(pt => pt.PlaylistId == playlistId && pt.MusicTrackId == trackId);
            if (item != null)
            {
                _context.PlaylistTracks.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
