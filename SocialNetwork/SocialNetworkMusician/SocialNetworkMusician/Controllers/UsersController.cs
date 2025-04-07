using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Data;
using Microsoft.EntityFrameworkCore;
using SocialNetworkMusician.Models;

namespace SocialNetworkMusician.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UsersController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users
                .Select(u => new UserProfileViewModel
                {
                    Id = u.Id,
                    DisplayName = u.DisplayName,
                    Email = u.Email,
                    JoinedDate = u.JoinedDate,
                    TrackCount = _context.MusicTracks.Count(t => t.UserId == u.Id)
                }).ToListAsync();

            return View(users);
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();

            var model = new UserProfileViewModel
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email,
                JoinedDate = user.JoinedDate,
                TrackCount = await _context.MusicTracks.CountAsync(t => t.UserId == user.Id)
            };

            return View(model);
        }
    }
}
