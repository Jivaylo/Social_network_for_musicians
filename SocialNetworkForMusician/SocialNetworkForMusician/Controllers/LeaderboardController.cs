using Microsoft.AspNetCore.Mvc;
using SocialNetworkForMusician.Data.Entities;
using SocialNetworkForMusician.Data;
using Microsoft.EntityFrameworkCore;

namespace SocialNetworkForMusician.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LeaderboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Leaderboard>>> GetLeaderboard()
        {
            return await _context.Leaderboards.Include(l => l.Song).ThenInclude(s => s.Artist).OrderByDescending(l => l.AverageRating).ToListAsync();
        }
    }
}

