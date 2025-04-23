using Microsoft.AspNetCore.Mvc;
using SocialNetworkMusician.Models;
using System.Diagnostics;

namespace SocialNetworkMusician.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var model = new ErrorViewModel
            {
                StatusCode = statusCode
            };

            switch (statusCode)
            {
                case 404:
                    model.Title = "Page Not Found";
                    model.Message = "The page you are looking for could not be found.";
                    break;
                case 500:
                    model.Title = "Server Error";
                    model.Message = "Oops! Something went wrong on our end.";
                    break;
                default:
                    model.Title = "Error";
                    model.Message = "An unexpected error occurred.";
                    break;
            }

            return View("ErrorPage", model);
        }

        [Route("Error")]
        public IActionResult GeneralError()
        {
            ViewBag.ErrorMessage = "An unknown error occurred.";
            return View("GenericError");
        }

        [Route("Error/ErrorPage")]
        public IActionResult ErrorPage()
        {
            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View("ErrorPage", model);
        }
    }
}
