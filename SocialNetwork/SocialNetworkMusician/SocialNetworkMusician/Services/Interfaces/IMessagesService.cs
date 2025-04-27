using SocialNetworkMusician.Models;

namespace SocialNetworkMusician.Services.Interfaces
{
    public interface IMessagesService
    {
        Task<List<MessageViewModel>> GetInboxMessagesAsync(string userId);
        Task<List<MessageViewModel>> GetSentMessagesAsync(string userId);
        Task SendMessageAsync(ComposeMessageViewModel model, string senderId);
        Task<MessageViewModel?> GetMessageDetailsAsync(Guid messageId, string currentUserId);
    }
}
