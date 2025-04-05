using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication_EVENTEASE_ST10448895.Models;

namespace WebApplication_EVENTEASE_ST10448895.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Venue)
                .Include(b => b.EventS)
                .ToListAsync();
            return View(bookings);
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Venue)
                .Include(b => b.EventS)
                .FirstOrDefaultAsync(m => m.Booking_ID == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewBag.Venue_ID = new SelectList(_context.Venue, "Venue_ID", "Venue_Name");
            ViewBag.Event_ID = new SelectList(_context.EventS, "Event_ID", "Event_Name");
            return View();
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Bookings bookings)
        {
            if (ModelState.IsValid)
            {
                if (bookings.Venue_ID == null || bookings.Event_ID == null)
                {
                    ModelState.AddModelError("", "Please select both Venue and Event.");
                }
                else
                {
                    _context.Bookings.Add(bookings);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.Venue_ID = new SelectList(_context.Venue, "Venue_ID", "Venue_Name", bookings.Venue_ID);
            ViewBag.Event_ID = new SelectList(_context.EventS, "Event_ID", "Event_Name", bookings.Event_ID);
            return View(bookings);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookings = await _context.Bookings
                .Include(b => b.Venue)
                .Include(b => b.EventS)
                .FirstOrDefaultAsync(b => b.Booking_ID == id);

            if (bookings == null)
            {
                return NotFound();
            }

            ViewBag.Venue_ID = new SelectList(_context.Venue, "Venue_ID", "Venue_Name", bookings.Venue_ID);
            ViewBag.Event_ID = new SelectList(_context.EventS, "Event_ID", "Event_Name", bookings.Event_ID);

            return View(bookings);
        }

        // POST: Bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Bookings bookings)
        {
            if (id != bookings.Booking_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookings);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(bookings.Booking_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Venue_ID = new SelectList(_context.Venue, "Venue_ID", "Venue_Name", bookings.Venue_ID);
            ViewBag.Event_ID = new SelectList(_context.EventS, "Event_ID", "Event_Name", bookings.Event_ID);

            return View(bookings);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookings = await _context.Bookings
                .Include(b => b.Venue)
                .Include(b => b.EventS)
                .FirstOrDefaultAsync(m => m.Booking_ID == id);

            if (bookings == null)
            {
                return NotFound();
            }

            return View(bookings);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookings = await _context.Bookings.FindAsync(id);
            if (bookings != null)
            {
                _context.Bookings.Remove(bookings);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Booking_ID == id);
        }
    }
}
