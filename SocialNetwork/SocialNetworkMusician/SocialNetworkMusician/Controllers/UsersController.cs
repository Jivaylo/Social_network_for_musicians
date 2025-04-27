using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkMusician.Models;
using SocialNetworkMusician.Services.Interfaces;
using SocialNetworkMusician.Data.Data;

namespace SocialNetworkMusician.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Profile(string id)
        {
            var model = await _userService.GetUserProfileAsync(id, User);
            if (model == null) return NotFound();

            return View("Profile", model);
        }

        [HttpPost]
        public async Task<IActionResult> Follow(string userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id == userId) return BadRequest();

            await _userService.FollowUserAsync(currentUser.Id, userId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Unfollow(string userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            await _userService.UnfollowUserAsync(currentUser.Id, userId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index(string search)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var users = await _userService.GetUsersListAsync(currentUser.Id, search);

            ViewBag.Search = search;
            return View(users);
        }

        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = _userService.GetEditProfileModel(user);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            await _userService.UpdateUserProfileAsync(user, model);

            return RedirectToAction("Profile", new { id = user.Id });
        }

        public async Task<IActionResult> MyProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            return RedirectToAction("Profile", new { id = user.Id });
        }
    }
}
