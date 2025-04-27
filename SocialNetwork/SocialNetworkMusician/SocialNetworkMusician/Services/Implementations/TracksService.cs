using Microsoft.AspNetCore.Identity;
using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Data;
using SocialNetworkMusician.Models;
using SocialNetworkMusician.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SocialNetworkMusician.Services.Implementations
{
    public class TracksService : ITracksService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TracksService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<TrackViewModel>> GetAllTracksAsync()
        {
            return await _context.MusicTracks
                .Include(t => t.User)
                .Include(t => t.Category)
                .Include(t => t.Likes)
                .Include(t => t.Dislikes)
                .Select(t => new TrackViewModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    FileUrl = t.FileUrl,
                    UploadedAt = t.UploadedAt,
                    UserName = t.User.UserName,
                    CategoryName = t.Category.Name,
                    LikeCount = t.Likes.Count,
                    DislikeCount = t.Dislikes.Count,
                    PlayCount = t.PlayCount,
                    ImageUrl = t.ImageUrl
                })
                .ToListAsync();
        }

        public async Task<TrackViewModel?> GetTrackDetailsAsync(Guid id, string? currentUserId)
        {
            var track = await _context.MusicTracks
                .Include(t => t.User)
                .Include(t => t.Category)
                .Include(t => t.Likes)
                .Include(t => t.Dislikes)
                .Include(t => t.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (track == null)
                return null;

            return new TrackViewModel
            {
                Id = track.Id,
                Title = track.Title,
                Description = track.Description,
                FileUrl = track.FileUrl,
                CategoryName = track.Category?.Name,
                UploadedAt = track.UploadedAt,
                UserName = track.User?.UserName,
                LikeCount = track.Likes.Count,
                DislikeCount = track.Dislikes.Count,
                PlayCount = track.PlayCount,
                IsLikedByCurrentUser = currentUserId != null && track.Likes.Any(l => l.UserId == currentUserId),
                IsDislikedByCurrentUser = currentUserId != null && track.Dislikes.Any(d => d.UserId == currentUserId),
                Comments = track.Comments.Select(c => new CommentViewModel
                {
                    UserName = c.User.UserName,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt
                }).OrderByDescending(c => c.CreatedAt).ToList()
            };
        }

        public async Task CreateTrackAsync(TrackViewModel model, string userId)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            Directory.CreateDirectory(uploadsFolder);

            string? fileUrl = null;
            if (model.MusicFile != null)
            {
                var uniqueFileName = Guid.NewGuid() + Path.GetExtension(model.MusicFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.MusicFile.CopyToAsync(stream);
                }
                fileUrl = "/uploads/" + uniqueFileName;
            }
            else if (!string.IsNullOrWhiteSpace(model.FileUrl))
            {
                fileUrl = model.FileUrl;
            }

            string? imageUrl = null;
            if (model.TrackImage != null)
            {
                var imageFileName = Guid.NewGuid() + Path.GetExtension(model.TrackImage.FileName);
                var imagePath = Path.Combine(uploadsFolder, imageFileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await model.TrackImage.CopyToAsync(stream);
                }
                imageUrl = "/uploads/" + imageFileName;
            }

            var track = new MusicTrack
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Description = model.Description,
                FileUrl = fileUrl!,
                CategoryId = model.CategoryId,
                UploadedAt = DateTime.UtcNow,
                UserId = userId,
                ImageUrl = imageUrl
            };

            _context.MusicTracks.Add(track);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTrackAsync(Guid id, string userId, bool isAdmin)
        {
            var track = await _context.MusicTracks.FindAsync(id);
            if (track == null) return;

            if (track.UserId != userId && !isAdmin)
                throw new UnauthorizedAccessException();

            _context.MusicTracks.Remove(track);
            await _context.SaveChangesAsync();
        }

        public async Task LikeTrackAsync(Guid id, string userId)
        {
            var track = await _context.MusicTracks.Include(t => t.Likes).Include(t => t.Dislikes).FirstOrDefaultAsync(t => t.Id == id);
            if (track == null) return;

            var like = track.Likes.FirstOrDefault(l => l.UserId == userId);
            var dislike = track.Dislikes.FirstOrDefault(d => d.UserId == userId);

            if (like != null)
                _context.Likes.Remove(like);
            else
            {
                if (dislike != null)
                    _context.Dislikes.Remove(dislike);

                _context.Likes.Add(new Like { Id = Guid.NewGuid(), UserId = userId, MusicTrackId = id });
            }

            await _context.SaveChangesAsync();
        }

        public async Task DislikeTrackAsync(Guid id, string userId)
        {
            var track = await _context.MusicTracks.Include(t => t.Likes).Include(t => t.Dislikes).FirstOrDefaultAsync(t => t.Id == id);
            if (track == null) return;

            var dislike = track.Dislikes.FirstOrDefault(d => d.UserId == userId);
            var like = track.Likes.FirstOrDefault(l => l.UserId == userId);

            if (dislike != null)
                _context.Dislikes.Remove(dislike);
            else
            {
                if (like != null)
                    _context.Likes.Remove(like);

                _context.Dislikes.Add(new Dislike { Id = Guid.NewGuid(), UserId = userId, MusicTrackId = id });
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddCommentAsync(Guid id, string comment, string userId)
        {
            var track = await _context.MusicTracks.FindAsync(id);
            if (track == null) return;

            var cleanComment = Utilities.BadWordsFilter.CleanComment(comment);

            _context.Comments.Add(new Comment
            {
                Id = Guid.NewGuid(),
                Content = cleanComment,
                CreatedAt = DateTime.UtcNow,
                MusicTrackId = id,
                UserId = userId
            });

            await _context.SaveChangesAsync();
        }

        public async Task IncrementPlayCountAsync(Guid id)
        {
            var track = await _context.MusicTracks.FindAsync(id);
            if (track == null) return;

            track.PlayCount++;
            await _context.SaveChangesAsync();
        }

        public async Task<List<TrackViewModel>> GetTrendingTracksAsync()
        {
            var tracks = await _context.MusicTracks
                .Include(t => t.Likes)
                .Include(t => t.Dislikes)
                .Include(t => t.Category)
                .Include(t => t.User)
                .ToListAsync();

            return tracks.Select(t => new TrackViewModel
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                FileUrl = t.FileUrl,
                CategoryName = t.Category?.Name,
                UserName = t.User?.DisplayName,
                LikeCount = t.Likes.Count,
                DislikeCount = t.Dislikes.Count,
                PlayCount = t.PlayCount,
                UploadedAt = t.UploadedAt,
                ImageUrl = t.ImageUrl,
                Score = t.Likes.Count + (t.PlayCount * 0.5)
            })
            .OrderByDescending(t => t.Score)
            .ToList();
        }

        public async Task EditTrackAsync(Guid id, TrackViewModel model, string userId, bool isAdmin)
        {
            var track = await _context.MusicTracks.FindAsync(id);
            if (track == null) return;
            if (track.UserId != userId && !isAdmin) throw new UnauthorizedAccessException();

            track.Title = model.Title;
            track.Description = model.Description;
            track.CategoryId = model.CategoryId;

            if (model.TrackImage != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/images");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(model.TrackImage.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.TrackImage.CopyToAsync(stream);
                }

                track.ImageUrl = "/uploads/images/" + fileName;
            }

            await _context.SaveChangesAsync();
        }
    }
}
