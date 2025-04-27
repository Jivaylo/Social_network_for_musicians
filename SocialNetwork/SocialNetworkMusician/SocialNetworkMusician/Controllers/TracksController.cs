using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Models;
using SocialNetworkMusician.Services.Interfaces;

namespace SocialNetworkMusician.Controllers
{
    [Authorize]
    public class TracksController : Controller
    {
        private readonly ITracksService _tracksService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TracksController(ITracksService tracksService, UserManager<ApplicationUser> userManager)
        {
            _tracksService = tracksService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var tracks = await _tracksService.GetAllTracksAsync();
            return View(tracks);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            var model = await _tracksService.GetTrackDetailsAsync(id, user?.Id);

            if (model == null) return NotFound();

            ViewBag.UserPlaylists = user != null ? (await _userManager.Users.Include(u => u.Playlists).FirstOrDefaultAsync(u => u.Id == user.Id))?.Playlists : new List<Playlist>();
            return View(model);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(TrackViewModel model)
        {
            if (!ModelState.IsValid || (model.MusicFile == null && string.IsNullOrWhiteSpace(model.FileUrl)))
            {
                ModelState.AddModelError("", "Please upload a music file or provide a music link.");
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            await _tracksService.CreateTrackAsync(model, user.Id);
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            await _tracksService.DeleteTrackAsync(id, user.Id, User.IsInRole("Admin"));
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Like(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            await _tracksService.LikeTrackAsync(id, user.Id);
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Dislike(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            await _tracksService.DislikeTrackAsync(id, user.Id);
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment(Guid id, string newComment)
        {
            if (string.IsNullOrWhiteSpace(newComment))
                return RedirectToAction(nameof(Details), new { id });

            var user = await _userManager.GetUserAsync(User);
            await _tracksService.AddCommentAsync(id, newComment, user.Id);
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> IncrementPlayCount(Guid id)
        {
            await _tracksService.IncrementPlayCountAsync(id);
            return Ok();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Trending()
        {
            var tracks = await _tracksService.GetTrendingTracksAsync();
            return View(tracks);
        }

        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            var track = await _tracksService.GetTrackDetailsAsync(id, user?.Id);

            if (track == null) return NotFound();
            if (track.UserName != user.UserName && !User.IsInRole("Admin")) return Forbid();

            return View(track);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, TrackViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            await _tracksService.EditTrackAsync(id, model, user.Id, User.IsInRole("Admin"));

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
