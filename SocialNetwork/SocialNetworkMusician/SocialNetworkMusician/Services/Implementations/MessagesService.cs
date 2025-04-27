using Microsoft.EntityFrameworkCore;
using SocialNetworkMusician.Data;
using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Models;
using SocialNetworkMusician.Services.Interfaces;

namespace SocialNetworkMusician.Services.Implementations
{
    public class MessagesService : IMessagesService
    {
        private readonly ApplicationDbContext _context;

        public MessagesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MessageViewModel>> GetInboxMessagesAsync(string userId)
        {
            return await _context.Messages
                .Where(m => m.RecipientId == userId)
                .OrderByDescending(m => m.SentAt)
                .Include(m => m.Sender)
                .Select(m => new MessageViewModel
                {
                    Id = m.Id,
                    SenderName = m.Sender.DisplayName,
                    RecipientName = m.Recipient.DisplayName,
                    Content = m.Content,
                    SentAt = m.SentAt,
                    IsRead = m.IsRead
                })
                .ToListAsync();
        }

        public async Task<List<MessageViewModel>> GetSentMessagesAsync(string userId)
        {
            return await _context.Messages
                .Where(m => m.SenderId == userId)
                .OrderByDescending(m => m.SentAt)
                .Include(m => m.Recipient)
                .Select(m => new MessageViewModel
                {
                    Id = m.Id,
                    SenderName = m.Sender.DisplayName,
                    RecipientName = m.Recipient.DisplayName,
                    Content = m.Content,
                    SentAt = m.SentAt,
                    IsRead = true
                })
                .ToListAsync();
        }

        public async Task SendMessageAsync(ComposeMessageViewModel model, string senderId)
        {
            var message = new Message
            {
                Id = Guid.NewGuid(),
                SenderId = senderId,
                RecipientId = model.RecipientId,
                Content = model.Content,
                SentAt = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task<MessageViewModel?> GetMessageDetailsAsync(Guid messageId, string currentUserId)
        {
            var message = await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Recipient)
                .FirstOrDefaultAsync(m => m.Id == messageId &&
                    (m.RecipientId == currentUserId || m.SenderId == currentUserId));

            if (message == null) return null;

            if (!message.IsRead && message.RecipientId == currentUserId)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
            }

            return new MessageViewModel
            {
                Id = message.Id,
                SenderName = message.Sender.DisplayName,
                RecipientName = message.Recipient.DisplayName,
                Content = message.Content,
                SentAt = message.SentAt,
                IsRead = message.IsRead
            };
        }
    }
}
