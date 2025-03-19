using Microsoft.AspNetCore.Mvc;
using SocialNetworkForMusician.Data.Entities;
using SocialNetworkForMusician.Data;
using Microsoft.EntityFrameworkCore;

namespace SocialNetworkForMusician.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LikesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Like>>> GetLikes()
        {
            return await _context.Likes.Include(l => l.User).Include(l => l.Song).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Like>> CreateLike(Like like)
        {
            _context.Likes.Add(like);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetLikes", new { id = like.Id }, like);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLike(int id)
        {
            var like = await _context.Likes.FindAsync(id);
            if (like == null)
            {
                return NotFound();
            }

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

