using EventEaseWebApp.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventEaseWebApp.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventEaseDbContext _context;

        public EventsController(EventEaseDbContext context)
        {
            _context = context;
        }

        // GET: Events
        public IActionResult List()
        {
            var events = _context.Events.Include(e => e.Venue).ToList();
            return View(events);
        }

        // GET: Events/Details/5
        public IActionResult Details(int id)
        {
            var eventItem = _context.Events.Include(e => e.Venue).FirstOrDefault(e => e.EventId == id);
            if (eventItem == null)
            {
                return NotFound();
            }
            return View(eventItem);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName");
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("EventName,EventDate,Description,VenueId")] Event eventItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventItem);
                _context.SaveChanges();
                return RedirectToAction(nameof(List));
            }
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", eventItem.VenueId);
            return View(eventItem);
        }
    }
}
