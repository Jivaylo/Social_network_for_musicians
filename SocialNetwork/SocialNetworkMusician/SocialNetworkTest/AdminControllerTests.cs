using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SocialNetworkMusician.Controllers;
using SocialNetworkMusician.Models;
using SocialNetworkMusician.Services.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialNetworkTest
{
    [TestFixture]
    public class AdminControllerTests
    {
        private Mock<IAdminService> _adminServiceMock;
        private AdminController _controller;

        [SetUp]
        public void Setup()
        {
            _adminServiceMock = new Mock<IAdminService>();
            _controller = new AdminController(_adminServiceMock.Object);
        }

        [Test]
        public async Task Index_Returns_ViewResult_With_UserList()
        {
            // Arrange
            _adminServiceMock.Setup(x => x.GetAdminDashboardAsync("display", "asc"))
                .ReturnsAsync(new List<AdminUserViewModel> { new AdminUserViewModel { Id = "1", Email = "admin@test.com" } });

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult!.Model, Is.AssignableTo<List<AdminUserViewModel>>());
        }

        [Test]
        public async Task PromoteToAdmin_Calls_Service_And_Redirects()
        {
            // Act
            var result = await _controller.PromoteToAdmin("userId");

            // Assert
            _adminServiceMock.Verify(x => x.PromoteToAdminAsync("userId"), Times.Once);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirect = result as RedirectToActionResult;
            Assert.That(redirect!.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task BanUser_Calls_Service_And_Redirects()
        {
            // Act
            var result = await _controller.BanUser("userId");

            // Assert
            _adminServiceMock.Verify(x => x.BanUserAsync("userId"), Times.Once);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirect = result as RedirectToActionResult;
            Assert.That(redirect!.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task UnbanUser_Calls_Service_And_Redirects()
        {
            // Act
            var result = await _controller.UnbanUser("userId");

            // Assert
            _adminServiceMock.Verify(x => x.UnbanUserAsync("userId"), Times.Once);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirect = result as RedirectToActionResult;
            Assert.That(redirect!.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task Reports_Returns_ViewResult_With_ReportList()
        {
            // Arrange
            _adminServiceMock.Setup(x => x.GetReportsAsync())
                .ReturnsAsync(new List<ReportViewModel> { new ReportViewModel { Id = Guid.NewGuid() } });

            // Act
            var result = await _controller.Reports();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult!.Model, Is.AssignableTo<List<ReportViewModel>>());
        }

        [Test]
        public async Task PromoteToModerator_Calls_Service_And_Redirects()
        {
            // Act
            var result = await _controller.PromoteToModerator("userId");

            // Assert
            _adminServiceMock.Verify(x => x.PromoteToModeratorAsync("userId"), Times.Once);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirect = result as RedirectToActionResult;
            Assert.That(redirect!.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task UnpromoteFromAdmin_Calls_Service_And_Redirects()
        {
            // Act
            var result = await _controller.UnpromoteFromAdmin("userId");

            // Assert
            _adminServiceMock.Verify(x => x.UnpromoteFromAdminAsync("userId"), Times.Once);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirect = result as RedirectToActionResult;
            Assert.That(redirect!.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task UnpromoteFromModerator_Calls_Service_And_Redirects()
        {
            // Act
            var result = await _controller.UnpromoteFromModerator("userId");

            // Assert
            _adminServiceMock.Verify(x => x.UnpromoteFromModeratorAsync("userId"), Times.Once);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirect = result as RedirectToActionResult;
            Assert.That(redirect!.ActionName, Is.EqualTo("Index"));
        }
    }
}
