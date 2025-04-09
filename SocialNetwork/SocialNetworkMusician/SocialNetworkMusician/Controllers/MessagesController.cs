using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Data;
using SocialNetworkMusician.Models;
using Microsoft.EntityFrameworkCore;

namespace SocialNetworkMusician.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessagesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Inbox()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var messages = await _context.Messages
                .Where(m => m.RecipientId == currentUser.Id)
                .OrderByDescending(m => m.SentAt)
                .Include(m => m.Sender)
                .Select(m => new MessageViewModel
                {
                    Id = m.Id,
                    SenderName = m.Sender.DisplayName,
                    RecipientName = currentUser.DisplayName,
                    Content = m.Content,
                    SentAt = m.SentAt,
                    IsRead = m.IsRead
                })
                .ToListAsync();

            return View(messages);
        }

        public async Task<IActionResult> Sent()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var messages = await _context.Messages
                .Where(m => m.SenderId == currentUser.Id)
                .OrderByDescending(m => m.SentAt)
                .Include(m => m.Recipient)
                .Select(m => new MessageViewModel
                {
                    Id = m.Id,
                    SenderName = currentUser.DisplayName,
                    RecipientName = m.Recipient.DisplayName,
                    Content = m.Content,
                    SentAt = m.SentAt,
                    IsRead = true
                })
                .ToListAsync();

            return View(messages);
        }

        [HttpGet]
        public async Task<IActionResult> Compose(string recipientId)
        {
            var recipient = await _userManager.FindByIdAsync(recipientId);
            if (recipient == null) return NotFound();

            var model = new ComposeMessageViewModel
            {
                RecipientId = recipient.Id,
                RecipientName = recipient.DisplayName
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Compose(ComposeMessageViewModel model)
        {
            var sender = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                var message = new Message
                {
                    Id = Guid.NewGuid(),
                    SenderId = sender.Id,
                    RecipientId = model.RecipientId,
                    Content = model.Content
                };

                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                return RedirectToAction("Sent");
            }

            return View(model);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var message = await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Recipient)
                .FirstOrDefaultAsync(m => m.Id == id &&
                    (m.RecipientId == currentUser.Id || m.SenderId == currentUser.Id));

            if (message == null) return NotFound();

            if (!message.IsRead && message.RecipientId == currentUser.Id)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
            }

            var vm = new MessageViewModel
            {
                Id = message.Id,
                SenderName = message.Sender.DisplayName,
                RecipientName = message.Recipient.DisplayName,
                Content = message.Content,
                SentAt = message.SentAt,
                IsRead = message.IsRead
            };

            return View(vm);
        }
    }
}
