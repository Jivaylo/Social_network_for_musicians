using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetworkForMusician.Data;
using SocialNetworkForMusician.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetworkForMusician.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ArtistsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Artist>>> GetArtists()
        {
            return await _context.Artists.Include(a => a.User).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Artist>> GetArtist(string id)
        {
            var artist = await _context.Artists.Include(a => a.User).FirstOrDefaultAsync(a => a.UserId == id);
            if (artist == null)
            {
                return NotFound();
            }
            return artist;
        }

        [HttpPost]
        public async Task<ActionResult<Artist>> CreateArtist(Artist artist)
        {
            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetArtist), new { id = artist.UserId }, artist);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArtist(string id, Artist artist)
        {
            if (id != artist.UserId)
            {
                return BadRequest();
            }

            _context.Entry(artist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Artists.Any(e => e.UserId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtist(string id)
        {
            var artist = await _context.Artists.FindAsync(id);
            if (artist == null)
            {
                return NotFound();
            }

            _context.Artists.Remove(artist);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
