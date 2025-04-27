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
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SocialNetworkTest
{
    [TestFixture]
    public class MessagesControllerTests
    {
        private ApplicationDbContext _dbContext;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private MessagesController _controller;
        private ApplicationUser _testUser;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new ApplicationDbContext(options);

            var store = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            _testUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "testuser",
                DisplayName = "Test User",
                Email = "test@example.com"
            };

            _userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(_testUser);

            _controller = new MessagesController(_dbContext, _userManagerMock.Object);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task Inbox_ReturnsView_WithMessages()
        {
            // Arrange
            _dbContext.Messages.Add(new Message
            {
                Id = Guid.NewGuid(),
                SenderId = _testUser.Id,
                RecipientId = _testUser.Id,
                Content = "Test Message",
                SentAt = DateTime.UtcNow
            });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _controller.Inbox() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<MessageViewModel>>(result.Model);
        }

        [Test]
        public async Task Sent_ReturnsView_WithMessages()
        {
            // Arrange
            _dbContext.Messages.Add(new Message
            {
                Id = Guid.NewGuid(),
                SenderId = _testUser.Id,
                RecipientId = Guid.NewGuid().ToString(),
                Content = "Sent Message",
                SentAt = DateTime.UtcNow
            });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _controller.Sent() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<MessageViewModel>>(result.Model);
        }

        [Test]
        public async Task Compose_Get_ReturnsView()
        {
            // Arrange
            var recipient = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                DisplayName = "Recipient"
            };
            _userManagerMock.Setup(u => u.FindByIdAsync(recipient.Id)).ReturnsAsync(recipient);

            // Act
            var result = await _controller.Compose(recipient.Id) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ComposeMessageViewModel>(result.Model);
        }

        [Test]
        public async Task Compose_Post_Valid_RedirectsToSent()
        {
            // Arrange
            var model = new ComposeMessageViewModel
            {
                RecipientId = Guid.NewGuid().ToString(),
                Content = "Hello!"
            };

            // Act
            var result = await _controller.Compose(model) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Sent", result.ActionName);
        }

     
    }
}

