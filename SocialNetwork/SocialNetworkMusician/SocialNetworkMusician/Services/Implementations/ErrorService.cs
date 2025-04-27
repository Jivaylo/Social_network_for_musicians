using SocialNetworkMusician.Models;
using SocialNetworkMusician.Services.Interfaces;

namespace SocialNetworkMusician.Services.Implementations
{
    public class ErrorService : IErrorService
    {
        public ErrorViewModel GetErrorModel(int statusCode)
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

            return model;
        }

        public ErrorViewModel GetGeneralErrorModel(string requestId)
        {
            return new ErrorViewModel
            {
                RequestId = requestId
            };
        }
    }
}
