using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocialNetworkMusician.Data;
using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Models;
using System;

namespace SocialNetworkMusician.Controllers
{
    public class TracksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
       
        public TracksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var tracks = await _context.MusicTracks
                .Include(t => t.User)
                .Include(t => t.Category)
                .Select(t => new TrackViewModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    FileUrl = t.FileUrl,
                    CategoryName = t.Category != null ? t.Category.Name : null,
                    UploadedAt = t.UploadedAt,
                    UserName = t.User.UserName
                })
                .ToListAsync();

            return View(tracks);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            var currentUserId = user?.Id;

            var track = await _context.MusicTracks
                .Include(t => t.User)
                .Include(t => t.Category)
                .Include(t => t.Likes)
                .Include(t => t.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (track == null)
                return NotFound();

            var vm = new TrackViewModel
            {
                Id = track.Id,
                Title = track.Title,
                Description = track.Description,
                FileUrl = track.FileUrl,
                CategoryName = track.Category?.Name,
                UploadedAt = track.UploadedAt,
                UserName = track.User?.UserName,
                LikeCount = track.Likes.Count,
                IsLikedByCurrentUser = currentUserId != null && track.Likes.Any(l => l.UserId == currentUserId),
                Comments = track.Comments.Select(c => new CommentViewModel
                {
                    UserName = c.User.UserName,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt
                }).OrderByDescending(c => c.CreatedAt).ToList()
            };

            return View(vm);
        }

        [Authorize]
        [Authorize]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.TrackCategories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(TrackViewModel model, IFormFile MusicFile)
        {
            if (!ModelState.IsValid || MusicFile == null)
            {
                ViewBag.Categories = new SelectList(_context.TrackCategories, "Id", "Name");
                ModelState.AddModelError("", "Music file is required.");
                return View(model);
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(MusicFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await MusicFile.CopyToAsync(stream);
            }

            var user = await _userManager.GetUserAsync(User);

            var track = new MusicTrack
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Description = model.Description,
                FileUrl = "/uploads/" + uniqueFileName,
                CategoryId = model.CategoryId,
                UploadedAt = DateTime.UtcNow,
                UserId = user.Id
            };

            _context.MusicTracks.Add(track);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var track = await _context.MusicTracks.FindAsync(id);

            if (track == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (track.UserId != user.Id && !User.IsInRole("Admin"))
                return Forbid();

            _context.MusicTracks.Remove(track);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Like(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            var track = await _context.MusicTracks
                .Include(t => t.Likes)
                .Include(t => t.Dislikes)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (track == null) return NotFound();

            var like = track.Likes.FirstOrDefault(l => l.UserId == user.Id);
            var dislike = track.Dislikes.FirstOrDefault(d => d.UserId == user.Id);

            if (like != null)
                _context.Likes.Remove(like);
            else
            {
                if (dislike != null) _context.Dislikes.Remove(dislike);
                _context.Likes.Add(new Like { Id = Guid.NewGuid(), UserId = user.Id, MusicTrackId = id });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Dislike(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            var track = await _context.MusicTracks
                .Include(t => t.Dislikes)
                .Include(t => t.Likes)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (track == null)
                return NotFound();

            var dislike = track.Dislikes.FirstOrDefault(d => d.UserId == user.Id);
            var like = track.Likes.FirstOrDefault(l => l.UserId == user.Id);

            if (dislike != null)
                _context.Dislikes.Remove(dislike);
            else
            {
                if (like != null)
                    _context.Likes.Remove(like);

                _context.Dislikes.Add(new Dislike
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    MusicTrackId = id
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment(Guid id, string newComment)
        {
            if (string.IsNullOrWhiteSpace(newComment))
                return RedirectToAction(nameof(Details), new { id });

            var user = await _userManager.GetUserAsync(User);
            var track = await _context.MusicTracks.FindAsync(id);

            if (track == null)
                return NotFound();

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                Content = newComment,
                CreatedAt = DateTime.UtcNow,
                UserId = user.Id,
                MusicTrackId = id
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id });
        }

    }
}
