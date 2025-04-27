using SocialNetworkMusician.Models;

namespace SocialNetworkMusician.Services.Interfaces
{
    public interface IErrorService
    {
        ErrorViewModel GetErrorModel(int statusCode);
        ErrorViewModel GetGeneralErrorModel(string requestId);
    }
}
