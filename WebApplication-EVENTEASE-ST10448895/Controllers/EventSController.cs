using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication_EVENTEASE_ST10448895.Models;

namespace WebApplication_EVENTEASE_ST10448895.Controllers
{
    public class EventSController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventSController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchType, int? venueId, DateTime? startDate, DateTime? endDate)
        {
            var events = _context.EventS
                .Include(e => e.Venue)
                .Include(e => e.EventType)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchType))
                events = events.Where(e => e.EventType.Name == searchType);

            if (venueId.HasValue)
                events = events.Where(e => e.Venue_ID == venueId);

            if (startDate.HasValue && endDate.HasValue)
                events = events.Where(e => e.Event_Date >= startDate && e.Event_Date <= endDate);

            // ✅ Set ViewData for dropdowns
            ViewData["EventTypes"] = _context.EventType.ToList();
            ViewData["Venues"] = _context.Venue.ToList();

            // Optional: remember filters
            ViewData["SelectedType"] = searchType;
            ViewData["SelectedVenue"] = venueId;
            ViewData["StartDate"] = startDate?.ToString("yyyy-MM-dd");
            ViewData["EndDate"] = endDate?.ToString("yyyy-MM-dd");

            return View(await events.ToListAsync());
        }


        public IActionResult CREATE()
        {
            ViewBag.Venue_ID = new SelectList(_context.Venue, "Venue_ID", "Locations");
            ViewBag.EventTypeID = new SelectList(_context.EventType, "EventTypeID", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CREATE(EventS events)
        {
            if (ModelState.IsValid)
            {
                bool isBooked = await _context.EventS.AnyAsync(e =>
                    e.Venue_ID == events.Venue_ID &&
                    e.Event_Date == events.Event_Date);

                if (isBooked)
                {
                    ModelState.AddModelError("", "This venue is already booked for the selected date and time.");
                }
                else
                {
                    _context.Add(events);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Event created successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.Venue_ID = new SelectList(_context.Venue, "Venue_ID", "Locations", events.Venue_ID);
            ViewBag.EventTypeID = new SelectList(_context.EventType, "EventTypeID", "Name");
            return View(events);
        }


        public async Task<IActionResult> Details(int? id)
        {
            var events = await _context.EventS.Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.Event_ID == id);
            if (events == null) return NotFound();
            return View(events);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var events = await _context.EventS.Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.Event_ID == id);
            if (events == null) return NotFound();
            return View(events);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var events = await _context.EventS.FindAsync(id);

            if (events == null)
            {
                return NotFound();
            }

            bool hasBookings = await _context.Bookings.AnyAsync(b => b.Event_ID == id);
            if (hasBookings)
            {
                TempData["ErrorMessage"] = "Cannot delete event. It has active bookings.";
                return View(events);
            }

            _context.EventS.Remove(events);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Event deleted successfully.";
            return RedirectToAction(nameof(Index));
        }




        private bool EventExists(int id)
        {
            return _context.EventS.Any(e => e.Event_ID == id);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var events = await _context.EventS.FindAsync(id);
            if (events == null) return NotFound();

            ViewBag.Venue_ID = new SelectList(_context.Venue, "Venue_ID", "Locations", events.Venue_ID);
            ViewBag.EventTypeID = new SelectList(_context.EventType, "EventTypeID", "Name");
            return View(events);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EventS events)
        {
            if (id != events.Event_ID) return NotFound();

            if (ModelState.IsValid)
            {
                bool isBooked = await _context.EventS.AnyAsync(e =>
                    e.Venue_ID == events.Venue_ID &&
                    e.Event_Date == events.Event_Date &&
                    e.Event_ID != events.Event_ID);

                if (isBooked)
                {
                    ModelState.AddModelError("", "This venue is already booked for the selected date and time.");
                }
                else
                {
                    try
                    {
                        _context.Update(events);
                        await _context.SaveChangesAsync();
                        TempData["SuccessMessage"] = "Event updated successfully.";
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EventExists(events.Event_ID)) return NotFound();
                        else throw;
                    }
                }
            }

            ViewBag.Venue_ID = new SelectList(_context.Venue, "Venue_ID", "Locations", events.Venue_ID);
            ViewBag.EventTypeID = new SelectList(_context.EventType, "EventTypeID", "Name");
            return View(events);
        }


    }
}

