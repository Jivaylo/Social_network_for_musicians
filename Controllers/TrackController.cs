using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Data;

namespace SocialNetworkMusician.Controllers
{
    public class TrackController : Controller
    {
        [Authorize]
        public class TrackController : Controller
        {
            private readonly ApplicationDbContext _context;
            private readonly UserManager<ApplicationUser> _userManager;

            public TrackController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
            {
                _context = context;
                _userManager = userManager;
            }

            // GET: Track
            public async Task<IActionResult> Index()
            {
                var tracks = _context.Tracks
                    .Include(t => t.Album)
                    .Include(t => t.Category)
                    .Include(t => t.User);

                return View(await tracks.ToListAsync());
            }

            // GET: Track/Details/5
            public async Task<IActionResult> Details(int? id)
            {
                if (id == null) return NotFound();

                var track = await _context.Tracks
                    .Include(t => t.Album)
                    .Include(t => t.Category)
                    .Include(t => t.User)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (track == null) return NotFound();

                return View(track);
            }

            // GET: Track/Create
            public IActionResult Create()
            {
                ViewData["AlbumId"] = new SelectList(_context.Albums, "Id", "Title");
                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
                return View();
            }

            // POST: Track/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create([Bind("Title,Url,AlbumId,CategoryId,Description")] Track track)
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    track.UserId = user.Id;
                    track.UploadedAt = DateTime.Now;

                    _context.Add(track);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                ViewData["AlbumId"] = new SelectList(_context.Albums, "Id", "Title", track.AlbumId);
                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", track.CategoryId);
                return View(track);
            }

            // GET: Track/Edit/5
            public async Task<IActionResult> Edit(int? id)
            {
                if (id == null) return NotFound();

                var track = await _context.Tracks.FindAsync(id);
                if (track == null) return NotFound();

                ViewData["AlbumId"] = new SelectList(_context.Albums, "Id", "Title", track.AlbumId);
                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", track.CategoryId);
                return View(track);
            }

            // POST: Track/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Url,AlbumId,CategoryId,Description,UploadedAt,UserId")] Track track)
            {
                if (id != track.Id) return NotFound();

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(track);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TrackExists(track.Id)) return NotFound();
                        else throw;
                    }
                    return RedirectToAction(nameof(Index));
                }

                ViewData["AlbumId"] = new SelectList(_context.Albums, "Id", "Title", track.AlbumId);
                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", track.CategoryId);
                return View(track);
            }

            // GET: Track/Delete/5
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null) return NotFound();

                var track = await _context.Tracks
                    .Include(t => t.Album)
                    .Include(t => t.Category)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (track == null) return NotFound();

                return View(track);
            }

            // POST: Track/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var track = await _context.Tracks.FindAsync(id);
                if (track != null)
                {
                    _context.Tracks.Remove(track);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            private bool TrackExists(int id)
            {
                return _context.Tracks.Any(e => e.Id == id);
            }
        }
    }
}
