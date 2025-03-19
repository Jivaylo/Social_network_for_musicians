using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetworkForMusician.Data.Entities;
using SocialNetworkForMusician.Data;

namespace SocialNetworkForMusician.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SongsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Song>>> GetSongs()
        {
            return await _context.Songs.Include(s => s.Artist).Include(s => s.Genre).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Song>> GetSong(int id)
        {
            var song = await _context.Songs.Include(s => s.Artist).Include(s => s.Genre).FirstOrDefaultAsync(s => s.Id == id);
            if (song == null)
            {
                return NotFound();
            }
            return song;
        }

        [HttpPost]
        public async Task<ActionResult<Song>> CreateSong(Song song)
        {
            _context.Songs.Add(song);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSong), new { id = song.Id }, song);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSong(int id, Song song)
        {
            if (id != song.Id)
            {
                return BadRequest();
            }

            _context.Entry(song).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Songs.Any(e => e.Id == id))
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
        public async Task<IActionResult> DeleteSong(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound();
            }

            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
