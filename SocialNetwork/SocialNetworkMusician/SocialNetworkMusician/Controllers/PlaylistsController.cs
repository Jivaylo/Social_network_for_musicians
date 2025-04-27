using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Models;
using SocialNetworkMusician.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SocialNetworkMusician.Controllers
{
    [Authorize]
    public class PlaylistsController : Controller
    {
        private readonly IPlaylistsService _playlistsService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PlaylistsController(IPlaylistsService playlistsService, UserManager<ApplicationUser> userManager)
        {
            _playlistsService = playlistsService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var playlists = await _playlistsService.GetUserPlaylistsAsync(user.Id);
            return View(playlists);
        }

        public async Task<IActionResult> Details(Guid id, string? search)
        {
            var (model, availableTracks) = await _playlistsService.GetPlaylistDetailsAsync(id, search);

            if (model == null) return NotFound();

            ViewBag.AvailableTracks = availableTracks;
            ViewBag.Search = search;

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            await _playlistsService.DeletePlaylistAsync(id, user.Id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            var user = await _userManager.GetUserAsync(User);
            await _playlistsService.CreatePlaylistAsync(name, user.Id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddTrack(Guid playlistId, Guid trackId)
        {
            await _playlistsService.AddTrackToPlaylistAsync(playlistId, trackId);
            return RedirectToAction("Details", new { id = playlistId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveTrack(Guid playlistId, Guid trackId)
        {
            await _playlistsService.RemoveTrackFromPlaylistAsync(playlistId, trackId);
            return RedirectToAction("Details", new { id = playlistId });
        }
    }
}
