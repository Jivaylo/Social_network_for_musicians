/*using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
    public class ReportsControllerTests
    {
        private ReportsController _controller;
        private ApplicationDbContext _dbContext;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private ApplicationUser _testUser;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);

            var store = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            _testUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@example.com",
                DisplayName = "Test User"
            };

            _dbContext.Users.Add(_testUser);
            _dbContext.SaveChanges();

            _userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(_testUser);

            var fakeIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, _testUser.Id),
                new Claim(ClaimTypes.Email, _testUser.Email)
            }, "TestAuth");

            var fakeUser = new ClaimsPrincipal(fakeIdentity);

            _controller = new ReportsController(_dbContext, _userManagerMock.Object);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = fakeUser }
            };
        }

        [Test]
        public void Create_Get_ReturnsViewWithModel()
        {
            // Act
            var result = _controller.Create(Guid.NewGuid(), "user123") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ReportViewModel>(result.Model);
        }

        [Test]
        public async Task Create_Post_ValidReport_RedirectsToTracks()
        {
            // Arrange
            var model = new ReportViewModel
            {
                Reason = "Spam",
                TrackId = Guid.NewGuid()
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
        new Claim(ClaimTypes.NameIdentifier, _testUser.Id),
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            _controller.TempData = new TempDataDictionary(
                new DefaultHttpContext(),
                Mock.Of<ITempDataProvider>()
            );

            // Act
            var result = await _controller.Create(model) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Tracks", result.ControllerName);
        }



        [Test]
        public void ReportTrack_Get_ReturnsView()
        {
            var result = _controller.ReportTrack(Guid.NewGuid()) as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ReportViewModel>(result.Model);
        }

        [Test]
        public async Task ReportTrack_Post_ValidModel_CreatesReport()
        {
            // Arrange
            var model = new ReportViewModel
            {
                TrackId = Guid.NewGuid(),
                Reason = "Inappropriate content"
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
        new Claim(ClaimTypes.NameIdentifier, _testUser.Id),
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            _controller.TempData = new TempDataDictionary(
                new DefaultHttpContext(),
                Mock.Of<ITempDataProvider>()
            );

            // Act
            var result = await _controller.ReportTrack(model) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Tracks", result.ControllerName);
        }


        [Test]
        public void ReportUser_Get_ReturnsView()
        {
            var result = _controller.ReportUser("user123") as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ReportViewModel>(result.Model);
        }

        [Test]
        public async Task ReportUser_Post_ValidModel_CreatesReport()
        {
            // Arrange
            var model = new ReportViewModel
            {
                ReportedUserId = Guid.NewGuid().ToString(),
                Reason = "Bad behavior"
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
        new Claim(ClaimTypes.NameIdentifier, _testUser.Id),
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            _controller.TempData = new TempDataDictionary(
                new DefaultHttpContext(),
                Mock.Of<ITempDataProvider>()
            );

            // Act
            var result = await _controller.ReportUser(model) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Users", result.ControllerName);
        }

    }
}*/
