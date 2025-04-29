using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SocialNetworkMusician.Controllers;
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
    public class MessagesControllerTests
    {
        private Mock<IMessagesService> _messagesServiceMock;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private MessagesController _controller;
        private ApplicationUser _testUser;

        [SetUp]
        public void Setup()
        {
            _messagesServiceMock = new Mock<IMessagesService>();

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

            _controller = new MessagesController(_messagesServiceMock.Object, _userManagerMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, _testUser.Id)
                        }))
                    }
                }
            };
        }

        [Test]
        public async Task Inbox_ReturnsView_WithInboxMessages()
        {
            _messagesServiceMock.Setup(x => x.GetInboxMessagesAsync(_testUser.Id))
                .ReturnsAsync(new List<MessageViewModel>());

            var result = await _controller.Inbox();

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsAssignableFrom<List<MessageViewModel>>(viewResult!.Model);
        }

        [Test]
        public async Task Sent_ReturnsView_WithSentMessages()
        {
            _messagesServiceMock.Setup(x => x.GetSentMessagesAsync(_testUser.Id))
                .ReturnsAsync(new List<MessageViewModel>());

            var result = await _controller.Sent();

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsAssignableFrom<List<MessageViewModel>>(viewResult!.Model);
        }

        [Test]
        public async Task Compose_Get_ValidRecipient_ReturnsView()
        {
            var recipient = new ApplicationUser { Id = Guid.NewGuid().ToString(), DisplayName = "Recipient" };
            _userManagerMock.Setup(x => x.FindByIdAsync(recipient.Id)).ReturnsAsync(recipient);

            var result = await _controller.Compose(recipient.Id);

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsAssignableFrom<ComposeMessageViewModel>(viewResult!.Model);
        }

        [Test]
        public async Task Compose_Get_RecipientNotFound_ReturnsNotFound()
        {
            _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null!);

            var result = await _controller.Compose(Guid.NewGuid().ToString());

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

      
        [Test]
        public async Task Details_MessageExists_ReturnsView()
        {
            var messageId = Guid.NewGuid();
            var message = new MessageViewModel { Id = messageId };

            _messagesServiceMock.Setup(x => x.GetMessageDetailsAsync(messageId, _testUser.Id))
                .ReturnsAsync(message);

            var result = await _controller.Details(messageId);

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(message, viewResult!.Model);
        }

        [Test]
        public async Task Details_MessageNotFound_ReturnsNotFound()
        {
            var messageId = Guid.NewGuid();

            _messagesServiceMock.Setup(x => x.GetMessageDetailsAsync(messageId, _testUser.Id))
                .ReturnsAsync((MessageViewModel)null!);

            var result = await _controller.Details(messageId);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
