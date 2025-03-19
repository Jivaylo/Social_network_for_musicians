using Microsoft.AspNetCore.Mvc;
using SocialNetworkForMusician.Data.Entities;
using SocialNetworkForMusician.Data;
using Microsoft.EntityFrameworkCore;

namespace SocialNetworkForMusician.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RatingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rating>>> GetRatings()
        {
            return await _context.Ratings.Include(r => r.User).Include(r => r.Song).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Rating>> CreateRating(Rating rating)
        {
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetRatings", new { id = rating.Id }, rating);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            var rating = await _context.Ratings.FindAsync(id);
            if (rating == null)
            {
                return NotFound();
            }

            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
