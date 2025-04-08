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


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Follow(string userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (userId == currentUser.Id) return BadRequest();

            var exists = await _context.Follows.AnyAsync(f =>
                f.FollowerId == currentUser.Id && f.FollowedId == userId);

            if (!exists)
            {
                _context.Follows.Add(new Follow
                {
                    FollowerId = currentUser.Id,
                    FollowedId = userId,
                    FollowedAt = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Unfollow(string userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var follow = await _context.Follows.FirstOrDefaultAsync(f =>
                f.FollowerId == currentUser.Id && f.FollowedId == userId);

            if (follow != null)
            {
                _context.Follows.Remove(follow);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<IActionResult> Index(string search)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var usersQuery = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                usersQuery = usersQuery.Where(u => u.Email.Contains(search));
            }

            var users = await usersQuery.ToListAsync();

            var model = new List<UserProfileViewModel>();

            foreach (var user in users)
            {
                if (user.Id == currentUser.Id) continue;

                var isFollowing = await _context.Follows.AnyAsync(f =>
                    f.FollowerId == currentUser.Id && f.FollowedId == user.Id);

                model.Add(new UserProfileViewModel
                {
                    Id = user.Id,
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    JoinedDate = user.JoinedDate,
                    IsFollowing = isFollowing
                });
            }

            ViewBag.Search = search;
            return View(model);
        }
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            var model = new EditProfileViewModel
            {
                DisplayName = user.DisplayName,
                Bio = user.Bio,
                ExistingImageUrl = user.ProfileImageUrl
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                user.DisplayName = model.DisplayName;
                user.Bio = model.Bio;

                if (model.ProfileImage != null)
                {
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/profile");
                    Directory.CreateDirectory(uploads);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ProfileImage.FileName);
                    var filePath = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ProfileImage.CopyToAsync(stream);
                    }

                    user.ProfileImageUrl = "/uploads/profile/" + fileName;
                }

                await _userManager.UpdateAsync(user);
                return RedirectToAction("Profile", new { id = user.Id });
            }

            return View(model);
        }
    }
}
