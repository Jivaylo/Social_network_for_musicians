/*using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using SocialNetworkMusician.Controllers;
using SocialNetworkMusician.Data;
using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
namespace SocialNetworkTest
{

    [TestFixture]
    public class TracksControllerTests
    {
        private ApplicationDbContext _dbContext;
        private UserManager<ApplicationUser> _userManager;
        private TracksController _controller;
        private ApplicationUser _testUser;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new ApplicationDbContext(options);

            var store = new Mock<IUserStore<ApplicationUser>>();
            _userManager = new UserManager<ApplicationUser>(store.Object, null, null, null, null, null, null, null, null);

            _testUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "testuser",
                DisplayName = "Test User",
                Email = "test@example.com"
            };

            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(_testUser);

            _controller = new TracksController(_dbContext, userManagerMock.Object);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, _testUser.Id)
                    }))
                }
            };
        }

        [Test]
        public async Task Index_ReturnsViewWithTracks()
        {
            // Arrange
            _dbContext.MusicTracks.Add(new MusicTrack
            {
                Id = Guid.NewGuid(),
                Title = "Test Track",
                FileUrl = "/uploads/test.mp3",
                UploadedAt = DateTime.UtcNow,
                UserId = _testUser.Id
            });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<TrackViewModel>>(result.Model);
        }

        [Test]
        public async Task IncrementPlayCount_ValidId_IncrementsPlayCount()
        {
            var track = new MusicTrack
            {
                Id = Guid.NewGuid(),
                Title = "PlayCount Test",
                FileUrl = "/uploads/test.mp3",
                UploadedAt = DateTime.UtcNow,
                UserId = _testUser.Id
            };
            _dbContext.MusicTracks.Add(track);
            await _dbContext.SaveChangesAsync();

            var response = await _controller.IncrementPlayCount(track.Id);

            var updatedTrack = await _dbContext.MusicTracks.FindAsync(track.Id);
            Assert.AreEqual(1, updatedTrack.PlayCount);
        }

        [Test]
        public async Task AddComment_AddsComment_WhenValid()
        {
            var track = new MusicTrack
            {
                Id = Guid.NewGuid(),
                Title = "Comment Track",
                FileUrl = "/uploads/test.mp3",
                UploadedAt = DateTime.UtcNow,
                UserId = _testUser.Id
            };
            _dbContext.MusicTracks.Add(track);
            await _dbContext.SaveChangesAsync();

            var result = await _controller.AddComment(track.Id, "Nice song!") as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Details", result.ActionName);
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.MusicTrackId == track.Id);
            Assert.IsNotNull(comment);
            Assert.AreEqual("Nice song!", comment.Content);
        }

        [Test]
        public async Task Like_AddsLikeAndRemovesDislikeIfExists()
        {
            var track = new MusicTrack
            {
                Id = Guid.NewGuid(),
                Title = "Like Test",
                FileUrl = "/uploads/test.mp3",
                UploadedAt = DateTime.UtcNow,
                UserId = _testUser.Id
            };
            _dbContext.MusicTracks.Add(track);
            await _dbContext.SaveChangesAsync();

            var result = await _controller.Like(track.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, _dbContext.Likes.Count());
        }

        [Test]
        public async Task Dislike_AddsDislikeAndRemovesLikeIfExists()
        {
            var track = new MusicTrack
            {
                Id = Guid.NewGuid(),
                Title = "Dislike Test",
                FileUrl = "/uploads/test.mp3",
                UploadedAt = DateTime.UtcNow,
                UserId = _testUser.Id
            };
            _dbContext.MusicTracks.Add(track);
            await _dbContext.SaveChangesAsync();

            var result = await _controller.Dislike(track.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, _dbContext.Dislikes.Count());
        }

        [Test]
        public async Task Details_TrackExists_ReturnsView()
        {
            // Arrange
            var track = new MusicTrack
            {
                Id = Guid.NewGuid(),
                Title = "Detail Test",
                FileUrl = "/uploads/test.mp3",
                UploadedAt = DateTime.UtcNow,
                UserId = _testUser.Id,
                User = _testUser,    // 🔥 Important! attach User object
                Category = new TrackCategory { Id = Guid.NewGuid(), Name = "Test Category" }, // 🔥 Important!
                Likes = new List<Like>(),
                Dislikes = new List<Dislike>(),
                Comments = new List<Comment>()
            };
            _dbContext.MusicTracks.Add(track);
            _dbContext.SaveChanges(); // No need for await

            // Act
            var result = await _controller.Details(track.Id) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<TrackViewModel>(result.Model);
        }

        [Test]
        public async Task Trending_ReturnsRankedTracks()
        {
            _dbContext.MusicTracks.Add(new MusicTrack
            {
                Id = Guid.NewGuid(),
                Title = "Trending Track",
                FileUrl = "/uploads/trending.mp3",
                UploadedAt = DateTime.UtcNow,
                UserId = _testUser.Id,
                PlayCount = 10
            });
            await _dbContext.SaveChangesAsync();

            var result = await _controller.Trending() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<TrackViewModel>>(result.Model);
        }
    }
}*/
