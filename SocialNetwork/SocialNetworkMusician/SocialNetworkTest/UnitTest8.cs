using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using SocialNetworkMusician.Controllers;
using SocialNetworkMusician.Models;

namespace SocialNetworkTest
{
    public class HomeControllerTests
    {
        private HomeController _controller;

        [SetUp]
        public void SetUp()
        {
            var loggerMock = new Moq.Mock<ILogger<HomeController>>();
            _controller = new HomeController(loggerMock.Object);
        }

        [Test]
        public void Index_ReturnsView()
        {
            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Error_ReturnsErrorViewModel()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = _controller.Error() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ErrorViewModel>(result.Model);
        }
    }
}
