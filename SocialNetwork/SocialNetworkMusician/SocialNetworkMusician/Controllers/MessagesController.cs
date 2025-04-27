using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Data;
using SocialNetworkMusician.Models;
using Microsoft.EntityFrameworkCore;
using SocialNetworkMusician.Services.Implementations;

namespace SocialNetworkMusician.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly MessagesService _messagesService;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessagesController(MessagesService messagesService, UserManager<ApplicationUser> userManager)
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

        [HttpPost]
        public async Task<IActionResult> Compose(ComposeMessageViewModel model)
        {
            var sender = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                // handle errors
                return View(model);
            }

            await _messagesService.SendMessageAsync(model, sender.Id);
            return RedirectToAction("Sent");
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var vm = await _messagesService.GetMessageDetailsAsync(id, currentUser.Id);
            if (vm == null) return NotFound();

            return View(vm);
        }
    }
}
