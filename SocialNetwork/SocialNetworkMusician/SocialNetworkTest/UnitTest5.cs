using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;
using SocialNetworkMusician.Controllers;
using SocialNetworkMusician.Models;
using SocialNetworkMusician.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using SocialNetworkMusician.Data.Data;

namespace SocialNetworkTest
{
    [TestFixture]
    public class ReportsControllerTests
    {
        private ReportsController _controller;
        private Mock<IReportsService> _reportsServiceMock;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private ApplicationUser _testUser;

        [SetUp]
        public void Setup()
        {
            _reportsServiceMock = new Mock<IReportsService>();

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            _testUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@example.com",
                DisplayName = "Test User"
            };

            _userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(_testUser);

            _controller = new ReportsController(_reportsServiceMock.Object, _userManagerMock.Object);

            var fakeIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, _testUser.Id),
                new Claim(ClaimTypes.Email, _testUser.Email)
            }, "TestAuth");

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(fakeIdentity) }
            };

            _controller.TempData = new TempDataDictionary(
                new DefaultHttpContext(),
                Mock.Of<ITempDataProvider>()
            );
        }

        [Test]
        public void Create_Get_ReturnsViewWithModel()
        {
            // Arrange
            _reportsServiceMock.Setup(r => r.PrepareCreateModel(It.IsAny<Guid?>(), It.IsAny<string>()))
                .Returns(new ReportViewModel());

            // Act
            var result = _controller.Create(Guid.NewGuid(), "user123") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ReportViewModel>(result.Model);
        }

        [Test]
        public async Task Create_Post_ValidModel_RedirectsToTracks()
        {
            // Arrange
            var model = new ReportViewModel
            {
                Reason = "Spam",
                TrackId = Guid.NewGuid()
            };

            // Act
            var result = await _controller.Create(model) as RedirectToActionResult;

            // Assert
            _reportsServiceMock.Verify(r => r.SubmitTrackReportAsync(model, _testUser.Id), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Tracks", result.ControllerName);
        }

        [Test]
        public void ReportTrack_Get_ReturnsView()
        {
            // Arrange
            _reportsServiceMock.Setup(r => r.PrepareReportTrackModel(It.IsAny<Guid>()))
                .Returns(new ReportViewModel());

            // Act
            var result = _controller.ReportTrack(Guid.NewGuid()) as ViewResult;

            // Assert
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
                Reason = "Inappropriate"
            };

            // Act
            var result = await _controller.ReportTrack(model) as RedirectToActionResult;

            // Assert
            _reportsServiceMock.Verify(r => r.SubmitTrackReportAsync(model, _testUser.Id), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Tracks", result.ControllerName);
        }

        [Test]
        public void ReportUser_Get_ReturnsView()
        {
            // Arrange
            _reportsServiceMock.Setup(r => r.PrepareReportUserModel(It.IsAny<string>()))
                .Returns(new ReportViewModel());

            // Act
            var result = _controller.ReportUser("user123") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ReportViewModel>(result.Model);
        }

        [Test]
        public async Task ReportUser_Post_ValidModel_CreatesReport()
        {
            // Arrange
            var model = new ReportViewModel
            {
                ReportedUserId = "user456",
                Reason = "Abuse"
            };

            // Act
            var result = await _controller.ReportUser(model) as RedirectToActionResult;

            // Assert
            _reportsServiceMock.Verify(r => r.SubmitUserReportAsync(model, _testUser.Id), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Users", result.ControllerName);
        }

        [Test]
        public async Task Index_ReturnsView_WithReportsList()
        {
            // Arrange
            _reportsServiceMock.Setup(r => r.GetAllReportsAsync())
                .ReturnsAsync(new List<ReportViewModel>());

            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<ReportViewModel>>(result.Model);
        }
    }
}
