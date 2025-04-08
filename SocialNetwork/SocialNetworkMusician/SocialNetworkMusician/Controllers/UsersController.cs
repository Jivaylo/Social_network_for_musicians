using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetworkMusician.Data;
using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Models;

namespace SocialNetworkMusician.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UsersController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        
        [AllowAnonymous]
        public async Task<IActionResult> Profile(string id)
        {
            var user = await _userManager.Users
                .Include(u => u.MusicTracks)
                .Include(u => u.Playlists)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            bool isFollowing = false;

            if (currentUser != null && currentUser.Id != user.Id)
            {
                isFollowing = await _context.Follows.AnyAsync(f =>
                    f.FollowerId == currentUser.Id && f.FollowedId == user.Id);
            }

            var followersCount = await _context.Follows.CountAsync(f => f.FollowedId == user.Id);
            var followingCount = await _context.Follows.CountAsync(f => f.FollowerId == user.Id);

            var vm = new UserProfileViewModel
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email,
                JoinedDate = user.JoinedDate,
                IsFollowing = isFollowing,
                FollowersCount = followersCount,
                FollowingCount = followingCount,

                Tracks = user.MusicTracks.Select(t => new TrackViewModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    FileUrl = t.FileUrl,
                    UploadedAt = t.UploadedAt,
                    CategoryName = t.Category?.Name
                }).ToList(),

                Playlists = user.Playlists.Select(p => new PlaylistViewModel
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToList()
            };

            return View("Profile", vm);
        }

        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Follow(string userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || currentUser.Id == userId)
                return BadRequest();

            var alreadyFollowing = await _context.Follows
                .AnyAsync(f => f.FollowerId == currentUser.Id && f.FollowedId == userId);

            if (!alreadyFollowing)
            {
                _context.Follows.Add(new Follow
                {
                    FollowerId = currentUser.Id,
                    FollowedId = userId,
                    FollowedAt = DateTime.UtcNow
                });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Profile", new { id = userId });
        }

   
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Unfollow(string userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return BadRequest();

            var follow = await _context.Follows
                .FirstOrDefaultAsync(f => f.FollowerId == currentUser.Id && f.FollowedId == userId);

            if (follow != null)
            {
                _context.Follows.Remove(follow);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Profile", new { id = userId });
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var userList = users.Select(u => new UserProfileViewModel
            {
                Id = u.Id,
                DisplayName = u.DisplayName,
                Email = u.Email,
                JoinedDate = u.JoinedDate,
                Tracks = _context.MusicTracks
                    .Where(t => t.UserId == u.Id)
                    .Select(t => new TrackViewModel
                    {
                        Id = t.Id,
                        Title = t.Title
                    }).ToList()
            }).ToList();

            return View(userList);
        }
    }
}
