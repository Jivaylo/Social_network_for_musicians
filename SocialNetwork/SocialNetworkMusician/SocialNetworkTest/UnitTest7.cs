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
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SocialNetworkTest
{
    [TestFixture]
    public class UsersControllerTests
    {
        private ApplicationDbContext _context;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private UsersController _controller;
        private ApplicationUser _testUser;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "UsersControllerTestsDb")
                .Options;
            _context = new ApplicationDbContext(options);

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            _testUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@example.com",
                DisplayName = "Test User"
            };

            _context.Users.Add(_testUser);
            _context.SaveChanges();

            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(_testUser);
            _userManagerMock.Setup(x => x.Users).Returns(_context.Users);

            _controller = new UsersController(_userManagerMock.Object, _context);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, _testUser.Id),
                new Claim(ClaimTypes.Name, _testUser.Email)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext { User = user }
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
            var result = await _controller.Profile(_testUser.Id) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Profile", result.ViewName);
            Assert.IsInstanceOf<UserProfileViewModel>(result.Model);
        }

        [Test]
        public async Task Index_ReturnsUsersList()
        {
            var result = await _controller.Index(null) as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<UserProfileViewModel>>(result.Model);
        }

        [Test]
        public async Task Follow_AddsFollow_RedirectsToIndex()
        {
            var anotherUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = "another@example.com",
                DisplayName = "Another User"
            };
            _context.Users.Add(anotherUser);
            _context.SaveChanges();

            var result = await _controller.Follow(anotherUser.Id) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);

            var followExists = await _context.Follows.AnyAsync(f => f.FollowerId == _testUser.Id && f.FollowedId == anotherUser.Id);
            Assert.IsTrue(followExists);
        }

        [Test]
        public async Task Unfollow_RemovesFollow_RedirectsToIndex()
        {
            var anotherUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = "another2@example.com",
                DisplayName = "Another User2"
            };
            _context.Users.Add(anotherUser);
            _context.Follows.Add(new Follow
            {
                FollowerId = _testUser.Id,
                FollowedId = anotherUser.Id,
                FollowedAt = DateTime.UtcNow
            });
            _context.SaveChanges();

            var result = await _controller.Unfollow(anotherUser.Id) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);

            var followStillExists = await _context.Follows.AnyAsync(f => f.FollowerId == _testUser.Id && f.FollowedId == anotherUser.Id);
            Assert.IsFalse(followStillExists);
        }

        [Test]
        public async Task EditProfile_Get_ReturnsViewWithUserData()
        {
            var result = await _controller.EditProfile() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<EditProfileViewModel>(result.Model);
        }

        [Test]
        public async Task EditProfile_Post_ValidModel_RedirectsToProfile()
        {
            var model = new EditProfileViewModel
            {
                DisplayName = "Updated Name",
                Bio = "Updated Bio"
            };

            var result = await _controller.EditProfile(model) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Profile", result.ActionName);

            var updatedUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == _testUser.Id);
            Assert.AreEqual("Updated Name", updatedUser.DisplayName);
        }

        [Test]
        public async Task MyProfile_RedirectsToProfile()
        {
            var result = await _controller.MyProfile() as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Profile", result.ActionName);
            Assert.AreEqual(_testUser.Id, result.RouteValues["id"]);
        }
    }
}
