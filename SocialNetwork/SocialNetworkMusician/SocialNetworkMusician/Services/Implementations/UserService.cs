using Microsoft.AspNetCore.Identity;
using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Data;
using SocialNetworkMusician.Models;
using SocialNetworkMusician.Services.Interfaces;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace SocialNetworkMusician.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<UserProfileViewModel?> GetUserProfileAsync(string id, ClaimsPrincipal currentUser)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            var currentUserId = _userManager.GetUserId(currentUser);

            bool isFollowing = currentUserId != null && await _context.Follows.AnyAsync(f => f.FollowerId == currentUserId && f.FollowedId == user.Id);

            var followersCount = await _context.Follows.CountAsync(f => f.FollowedId == user.Id);
            var followingCount = await _context.Follows.CountAsync(f => f.FollowerId == user.Id);

            var tracks = await _context.MusicTracks
                .Where(t => t.UserId == user.Id)
                .Include(t => t.Category)
                .Select(t => new TrackViewModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    FileUrl = t.FileUrl,
                    UploadedAt = t.UploadedAt,
                    CategoryName = t.Category!.Name
                }).ToListAsync();

            var playlists = await _context.Playlists
                .Where(p => p.UserId == user.Id)
                .Select(p => new PlaylistViewModel
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToListAsync();

            return new UserProfileViewModel
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email,
                JoinedDate = user.JoinedDate,
                IsFollowing = isFollowing,
                FollowersCount = followersCount,
                FollowingCount = followingCount,
                Tracks = tracks,
                Playlists = playlists
            };
        }

        public async Task FollowUserAsync(string currentUserId, string targetUserId)
        {
            var alreadyFollowing = await _context.Follows.AnyAsync(f => f.FollowerId == currentUserId && f.FollowedId == targetUserId);

            if (!alreadyFollowing)
            {
                _context.Follows.Add(new Follow
                {
                    FollowerId = currentUserId,
                    FollowedId = targetUserId,
                    FollowedAt = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
            }
        }

        public async Task UnfollowUserAsync(string currentUserId, string targetUserId)
        {
            var follow = await _context.Follows.FirstOrDefaultAsync(f => f.FollowerId == currentUserId && f.FollowedId == targetUserId);

            if (follow != null)
            {
                _context.Follows.Remove(follow);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<UserProfileViewModel>> GetUsersListAsync(string currentUserId, string? search)
        {
            var users = _userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                users = users.Where(u => u.Email.Contains(search));

            var userList = await users.ToListAsync();

            var model = new List<UserProfileViewModel>();

            foreach (var user in userList)
            {
                if (user.Id == currentUserId) continue;

                var isFollowing = await _context.Follows.AnyAsync(f => f.FollowerId == currentUserId && f.FollowedId == user.Id);

                model.Add(new UserProfileViewModel
                {
                    Id = user.Id,
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    JoinedDate = user.JoinedDate,
                    IsFollowing = isFollowing,
                    FollowersCount = await _context.Follows.CountAsync(f => f.FollowedId == user.Id),
                    FollowingCount = await _context.Follows.CountAsync(f => f.FollowerId == user.Id)
                });
            }

            return model;
        }

        public EditProfileViewModel GetEditProfileModel(ApplicationUser user)
        {
            return new EditProfileViewModel
            {
                DisplayName = user.DisplayName,
                Bio = user.Bio,
                ExistingImageUrl = user.ProfileImageUrl
            };
        }

        public async Task UpdateUserProfileAsync(ApplicationUser user, EditProfileViewModel model)
        {
            user.DisplayName = model.DisplayName;
            user.Bio = model.Bio;

            if (model.ProfileImage != null)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/profile");
                Directory.CreateDirectory(uploads);

                var fileName = Guid.NewGuid() + Path.GetExtension(model.ProfileImage.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(stream);
                }

                user.ProfileImageUrl = "/uploads/profile/" + fileName;
            }

            await _userManager.UpdateAsync(user);
        }
    }
}
