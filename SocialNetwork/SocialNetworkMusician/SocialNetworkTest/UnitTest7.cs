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
using System.Security.Claims;
using System.Threading.Tasks;

namespace SocialNetworkTest
{
    [TestFixture]
    public class UsersControllerTests
    {
        private ApplicationDbContext _context;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<IUserService> _userServiceMock;
        private UsersController _controller;
        private ApplicationUser _testUser;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _userServiceMock = new Mock<IUserService>();

            _testUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@example.com",
                DisplayName = "Test User"
            };

            _context.Users.Add(_testUser);
            _context.SaveChanges();

            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(_testUser);

            _controller = new UsersController(_userServiceMock.Object, _userManagerMock.Object);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, _testUser.Id)
                    }))
                }
            };
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task Profile_ValidId_ReturnsView()
        {
            // Arrange
            var expectedProfile = new UserProfileViewModel { Id = _testUser.Id, DisplayName = _testUser.DisplayName };
            _userServiceMock.Setup(s => s.GetUserProfileAsync(_testUser.Id, It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(expectedProfile);

            // Act
            var result = await _controller.Profile(_testUser.Id) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Profile", result.ViewName);
            Assert.IsInstanceOf<UserProfileViewModel>(result.Model);
        }

        [Test]
        public async Task Index_ReturnsUsersList()
        {
            // Arrange
            _userServiceMock.Setup(s => s.GetUsersListAsync(It.IsAny<string>(), null))
                .ReturnsAsync(new List<UserProfileViewModel>());

            // Act
            var result = await _controller.Index(null) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<UserProfileViewModel>>(result.Model);
        }

        [Test]
        public async Task Follow_AddsFollow_RedirectsToIndex()
        {
            // Arrange
            var anotherUserId = Guid.NewGuid().ToString();
            _userServiceMock.Setup(s => s.FollowUserAsync(_testUser.Id, anotherUserId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Follow(anotherUserId) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task Unfollow_RemovesFollow_RedirectsToIndex()
        {
            // Arrange
            var anotherUserId = Guid.NewGuid().ToString();
            _userServiceMock.Setup(s => s.UnfollowUserAsync(_testUser.Id, anotherUserId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Unfollow(anotherUserId) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task EditProfile_Get_ReturnsViewWithModel()
        {
            // Arrange
            _userServiceMock.Setup(s => s.GetEditProfileModel(_testUser))
                .Returns(new EditProfileViewModel { DisplayName = "Test User", Bio = "Bio Test" });

            // Act
            var result = await _controller.EditProfile() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<EditProfileViewModel>(result.Model);
        }

        [Test]
        public async Task EditProfile_Post_ValidModel_RedirectsToProfile()
        {
            // Arrange
            var model = new EditProfileViewModel
            {
                DisplayName = "Updated Name",
                Bio = "Updated Bio"
            };

            _userServiceMock.Setup(s => s.UpdateUserProfileAsync(_testUser, model))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.EditProfile(model) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Profile", result.ActionName);
            Assert.AreEqual(_testUser.Id, result.RouteValues["id"]);
        }

        [Test]
        public async Task MyProfile_RedirectsToProfile()
        {
            // Act
            var result = await _controller.MyProfile() as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Profile", result.ActionName);
            Assert.AreEqual(_testUser.Id, result.RouteValues["id"]);
        }
    }
}
