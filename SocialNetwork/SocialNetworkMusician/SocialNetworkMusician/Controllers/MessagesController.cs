using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Models;
using SocialNetworkMusician.Services.Interfaces;

namespace SocialNetworkMusician.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly IMessagesService _messagesService;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessagesController(IMessagesService messagesService, UserManager<ApplicationUser> userManager)
        {
            _messagesService = messagesService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Inbox()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var messages = await _messagesService.GetInboxMessagesAsync(currentUser.Id);
            return View(messages);
        }

        public async Task<IActionResult> Sent()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var messages = await _messagesService.GetSentMessagesAsync(currentUser.Id);
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
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "❌ Invalid input. Please fill all required fields.";
                return View(model);
            }

            var sender = await _userManager.GetUserAsync(User);
            await _messagesService.SendMessageAsync(model, sender.Id);

            TempData["Success"] = "✅ Message sent successfully!";
            return RedirectToAction("Sent");
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var message = await _messagesService.GetMessageDetailsAsync(id, currentUser.Id);

            if (message == null)
                return NotFound();

            return View(message);
        }
    }
}
