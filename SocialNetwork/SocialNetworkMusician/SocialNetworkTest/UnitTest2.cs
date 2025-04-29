using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SocialNetworkMusician.Controllers;
using SocialNetworkMusician.Models;
using SocialNetworkMusician.Services.Interfaces;
using System.Diagnostics;

namespace SocialNetworkTest
{
    [TestFixture]
    public class ErrorControllerTests
    {
        private Mock<IErrorService> _errorServiceMock;
        private ErrorController _controller;

        [SetUp]
        public void Setup()
        {
            _errorServiceMock = new Mock<IErrorService>();
            _controller = new ErrorController(_errorServiceMock.Object);
        }

        [Test]
        public void HttpStatusCodeHandler_Returns_ErrorPageView_WithCorrectModel_When404()
        {
            // Arrange
            var expectedModel = new ErrorViewModel
            {
                StatusCode = 404,
                Title = "Page Not Found",
                Message = "The page you are looking for could not be found."
            };

            _errorServiceMock.Setup(x => x.GetErrorModel(404)).Returns(expectedModel);

            // Act
            var result = _controller.HttpStatusCodeHandler(404) as ViewResult;
            var model = result?.Model as ErrorViewModel;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.ViewName, Is.EqualTo("ErrorPage"));
            Assert.That(model, Is.Not.Null);
            Assert.That(model!.StatusCode, Is.EqualTo(404));
            Assert.That(model.Title, Is.EqualTo("Page Not Found"));
            Assert.That(model.Message, Is.EqualTo("The page you are looking for could not be found."));
        }

        [Test]
        public void HttpStatusCodeHandler_Returns_ErrorPageView_WithCorrectModel_When500()
        {
            // Arrange
            var expectedModel = new ErrorViewModel
            {
                StatusCode = 500,
                Title = "Server Error",
                Message = "Oops! Something went wrong on our end."
            };

            _errorServiceMock.Setup(x => x.GetErrorModel(500)).Returns(expectedModel);

            // Act
            var result = _controller.HttpStatusCodeHandler(500) as ViewResult;
            var model = result?.Model as ErrorViewModel;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.ViewName, Is.EqualTo("ErrorPage"));
            Assert.That(model, Is.Not.Null);
            Assert.That(model!.StatusCode, Is.EqualTo(500));
            Assert.That(model.Title, Is.EqualTo("Server Error"));
            Assert.That(model.Message, Is.EqualTo("Oops! Something went wrong on our end."));
        }

        [Test]
        public void HttpStatusCodeHandler_Returns_ErrorPageView_With_GenericError_ForOtherCodes()
        {
            // Arrange
            var expectedModel = new ErrorViewModel
            {
                StatusCode = 123,
                Title = "Error",
                Message = "An unexpected error occurred."
            };

            _errorServiceMock.Setup(x => x.GetErrorModel(123)).Returns(expectedModel);

            // Act
            var result = _controller.HttpStatusCodeHandler(123) as ViewResult;
            var model = result?.Model as ErrorViewModel;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.ViewName, Is.EqualTo("ErrorPage"));
            Assert.That(model, Is.Not.Null);
            Assert.That(model!.StatusCode, Is.EqualTo(123));
            Assert.That(model.Title, Is.EqualTo("Error"));
            Assert.That(model.Message, Is.EqualTo("An unexpected error occurred."));
        }

        [Test]
        public void GeneralError_Returns_ErrorPageView()
        {
            // Arrange
            var requestId = "REQ123";
            var expectedModel = new ErrorViewModel { RequestId = requestId };

            _errorServiceMock.Setup(x => x.GetGeneralErrorModel(It.IsAny<string>())).Returns(expectedModel);

            // Fake setting HttpContext.TraceIdentifier
            var context = new DefaultHttpContext();
            context.TraceIdentifier = requestId;
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = context
            };

            // Act
            var result = _controller.GeneralError() as ViewResult;
            var model = result?.Model as ErrorViewModel;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.ViewName, Is.EqualTo("ErrorPage"));
            Assert.That(model, Is.Not.Null);
            Assert.That(model!.RequestId, Is.EqualTo(requestId));
        }

        [Test]
        public void ErrorPage_Returns_ErrorPageView()
        {
            // Arrange
            var requestId = "REQ456";
            var expectedModel = new ErrorViewModel { RequestId = requestId };

            _errorServiceMock.Setup(x => x.GetGeneralErrorModel(It.IsAny<string>())).Returns(expectedModel);

            // Fake setting HttpContext.TraceIdentifier
            var context = new DefaultHttpContext();
            context.TraceIdentifier = requestId;
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = context
            };

            // Act
            var result = _controller.ErrorPage() as ViewResult;
            var model = result?.Model as ErrorViewModel;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.ViewName, Is.EqualTo("ErrorPage"));
            Assert.That(model, Is.Not.Null);
            Assert.That(model!.RequestId, Is.EqualTo(requestId));
        }
    }
}
