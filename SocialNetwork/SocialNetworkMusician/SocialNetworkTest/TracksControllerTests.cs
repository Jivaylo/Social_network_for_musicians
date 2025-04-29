using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using SocialNetworkMusician.Controllers;
using SocialNetworkMusician.Data;
using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Models;
using SocialNetworkMusician.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SocialNetworkTest
{
    [TestFixture]
    public class TracksControllerTests
    {
        private Mock<ITracksService> _tracksServiceMock;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private ApplicationDbContext _dbContext;
        private TracksController _controller;
        private ApplicationUser _testUser;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new ApplicationDbContext(options);

            _tracksServiceMock = new Mock<ITracksService>();

            var userStore = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);

            _testUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "testuser",
                DisplayName = "Test User",
                Email = "test@example.com",
                Playlists = new List<Playlist>() // Important to mock playlists
            };

           
            var userList = new List<ApplicationUser> { _testUser }.AsQueryable();

            var usersMock = new Mock<DbSet<ApplicationUser>>();
            usersMock.As<IQueryable<ApplicationUser>>().Setup(m => m.Provider).Returns(userList.Provider);
            usersMock.As<IQueryable<ApplicationUser>>().Setup(m => m.Expression).Returns(userList.Expression);
            usersMock.As<IQueryable<ApplicationUser>>().Setup(m => m.ElementType).Returns(userList.ElementType);
            usersMock.As<IQueryable<ApplicationUser>>().Setup(m => m.GetEnumerator()).Returns(userList.GetEnumerator());

            _userManagerMock.Setup(x => x.Users).Returns(usersMock.Object);

            _userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(_testUser);

            _controller = new TracksController(_tracksServiceMock.Object, _userManagerMock.Object, _dbContext);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                new Claim(ClaimTypes.NameIdentifier, _testUser.Id)
            }))
                }
            };
        }


        [Test]
        public async Task Index_ReturnsViewWithTracks()
        {
            _tracksServiceMock.Setup(x => x.GetAllTracksAsync())
                .ReturnsAsync(new List<TrackViewModel>());

            var result = await _controller.Index() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<TrackViewModel>>(result.Model);
        }

        [Test]
        public async Task Like_AddsLikeAndRedirects()
        {
            var trackId = Guid.NewGuid();

            var result = await _controller.Like(trackId) as RedirectToActionResult;

            _tracksServiceMock.Verify(x => x.LikeTrackAsync(trackId, _testUser.Id), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual("Details", result.ActionName);
        }

        [Test]
        public async Task Dislike_AddsDislikeAndRedirects()
        {
            var trackId = Guid.NewGuid();

            var result = await _controller.Dislike(trackId) as RedirectToActionResult;

            _tracksServiceMock.Verify(x => x.DislikeTrackAsync(trackId, _testUser.Id), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual("Details", result.ActionName);
        }

        [Test]
        public async Task AddComment_AddsCommentAndRedirects()
        {
            var trackId = Guid.NewGuid();
            string comment = "Cool track!";

            var result = await _controller.AddComment(trackId, comment) as RedirectToActionResult;

            _tracksServiceMock.Verify(x => x.AddCommentAsync(trackId, comment, _testUser.Id), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual("Details", result.ActionName);
        }

        [Test]
        public async Task IncrementPlayCount_ReturnsOk()
        {
            var trackId = Guid.NewGuid();

            var result = await _controller.IncrementPlayCount(trackId) as OkResult;

            _tracksServiceMock.Verify(x => x.IncrementPlayCountAsync(trackId), Times.Once);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Trending_ReturnsTrendingTracks()
        {
            _tracksServiceMock.Setup(x => x.GetTrendingTracksAsync())
                .ReturnsAsync(new List<TrackViewModel>());

            var result = await _controller.Trending() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<TrackViewModel>>(result.Model);
        }

        [Test]
        public async Task Delete_DeletesTrackAndRedirects()
        {
            var trackId = Guid.NewGuid();

            var result = await _controller.Delete(trackId) as RedirectToActionResult;

            _tracksServiceMock.Verify(x => x.DeleteTrackAsync(trackId, _testUser.Id, It.IsAny<bool>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }
    }
}
