using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SocialNetworkMusician.Controllers;
using SocialNetworkMusician.Data;
using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Models;
using Xunit;

namespace SocialNetworkTest
{
    [TestFixture]
    public class AdminControllerTests
    {
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<IEmailSender> _emailSenderMock;
        private ApplicationDbContext _context;
        private AdminController _controller;

        [SetUp]
        public void Setup()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _emailSenderMock = new Mock<IEmailSender>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Unique DB for every test
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new AdminController(_context, _userManagerMock.Object, _emailSenderMock.Object);
        }

        [Test]
        public async Task Index_Returns_ViewResult_With_UserList()
        {
            // Arrange
            var user = new ApplicationUser { Id = "1", Email = "test@test.com", DisplayName = "Test User" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            _userManagerMock.Setup(x => x.Users).Returns(_context.Users);
            _userManagerMock.Setup(x => x.IsInRoleAsync(It.IsAny<ApplicationUser>(), "Admin")).ReturnsAsync(true);

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsInstanceOf<List<AdminUserViewModel>>(viewResult!.Model);
            Assert.That(((List<AdminUserViewModel>)viewResult.Model).Count, Is.EqualTo(1));
        }

        [Test]
        public async Task PromoteToAdmin_Should_Add_AdminRole_And_SendEmail()
        {
            // Arrange
            var user = new ApplicationUser { Id = "2", Email = "user@test.com", DisplayName = "User" };
            _userManagerMock.Setup(x => x.FindByIdAsync("2")).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.IsInRoleAsync(user, "Admin")).ReturnsAsync(false);
            _userManagerMock.Setup(x => x.AddToRoleAsync(user, "Admin")).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.PromoteToAdmin("2");

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            _emailSenderMock.Verify(x => x.SendEmailAsync(user.Email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task BanUser_Should_Lockout_User_And_SendEmail()
        {
            // Arrange
            var user = new ApplicationUser { Id = "3", Email = "ban@test.com", DisplayName = "Banned" };
            _userManagerMock.Setup(x => x.FindByIdAsync("3")).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.BanUser("3");

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.IsTrue(user.LockoutEnabled);
            _emailSenderMock.Verify(x => x.SendEmailAsync(user.Email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task UnbanUser_Should_Remove_Lockout_And_SendEmail()
        {
            // Arrange
            var user = new ApplicationUser { Id = "4", Email = "unban@test.com", DisplayName = "Unbanned" };
            _userManagerMock.Setup(x => x.FindByIdAsync("4")).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.UnbanUser("4");

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.IsNull(user.LockoutEnd);
            _emailSenderMock.Verify(x => x.SendEmailAsync(user.Email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Reports_Should_Return_List_Of_Reports()
        {
            // Arrange
            var reporter = new ApplicationUser { Id = "rep1", Email = "reporter@test.com" };
            var reported = new ApplicationUser { Id = "rep2", Email = "reported@test.com" };
            _context.Users.AddRange(reporter, reported);
            _context.Reports.Add(new Report
            {
                Id = Guid.NewGuid(),
                ReporterId = reporter.Id,
                ReportedUserId = reported.Id,
                Reason = "Spam",
                ReportedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Reports();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsInstanceOf<List<ReportViewModel>>(viewResult!.Model);
            Assert.That(((List<ReportViewModel>)viewResult.Model).Count, Is.EqualTo(1));
        }
    }
}