using NUnit.Framework;
using SocialNetworkMusician.Controllers;
using SocialNetworkMusician.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using SocialNetworkMusician.Services;

namespace SocialNetworkTest
{
    [TestFixture]
    public class ErrorControllerTests
    {
        private ErrorController _controller;

        [SetUp]
        public void SetUp()
        {
            _controller = new ErrorController();
        }

        [Test]
        public void HttpStatusCodeHandler_ReturnsErrorPageView_WithCorrectModel_When404()
        {
            // Act
            var result = _controller.HttpStatusCodeHandler(404) as ViewResult;
            var model = result?.Model as ErrorViewModel;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("ErrorPage", result.ViewName);
            Assert.IsNotNull(model);
            Assert.AreEqual(404, model.StatusCode);
            Assert.AreEqual("Page Not Found", model.Title);
            Assert.AreEqual("The page you are looking for could not be found.", model.Message);
        }

        [Test]
        public void HttpStatusCodeHandler_ReturnsErrorPageView_WithCorrectModel_When500()
        {
            // Act
            var result = _controller.HttpStatusCodeHandler(500) as ViewResult;
            var model = result?.Model as ErrorViewModel;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("ErrorPage", result.ViewName);
            Assert.IsNotNull(model);
            Assert.AreEqual(500, model.StatusCode);
            Assert.AreEqual("Server Error", model.Title);
            Assert.AreEqual("Oops! Something went wrong on our end.", model.Message);
        }

        [Test]
        public void HttpStatusCodeHandler_ReturnsErrorPageView_WithGenericError_ForOtherCodes()
        {
            // Act
            var result = _controller.HttpStatusCodeHandler(123) as ViewResult;
            var model = result?.Model as ErrorViewModel;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("ErrorPage", result.ViewName);
            Assert.IsNotNull(model);
            Assert.AreEqual(123, model.StatusCode);
            Assert.AreEqual("Error", model.Title);
            Assert.AreEqual("An unexpected error occurred.", model.Message);
        }

        [Test]
        public void GeneralError_ReturnsGenericErrorView()
        {
            // Act
            var result = _controller.GeneralError() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("GenericError", result.ViewName);
            Assert.IsNotNull(result.ViewData);
        }

       

    }
}
