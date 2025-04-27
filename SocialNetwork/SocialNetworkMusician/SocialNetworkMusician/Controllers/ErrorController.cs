using Microsoft.AspNetCore.Mvc;
using SocialNetworkMusician.Models;
using SocialNetworkMusician.Services;
using SocialNetworkMusician.Services.Interfaces;
using System.Diagnostics;

namespace SocialNetworkMusician.Controllers
{
    public class ErrorController : Controller
    {
        private readonly IErrorService _errorService;

        public ErrorController(IErrorService errorService)
        {
            _errorService = errorService;
        }

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var model = _errorService.GetErrorModel(statusCode);
            return View("ErrorPage", model);
        }

        [Route("Error")]
        public IActionResult GeneralError()
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            var model = _errorService.GetGeneralErrorModel(requestId);
            return View("ErrorPage", model);
        }

        [Route("Error/ErrorPage")]
        public IActionResult ErrorPage()
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            var model = _errorService.GetGeneralErrorModel(requestId);
            return View("ErrorPage", model);
        }
    }
}
