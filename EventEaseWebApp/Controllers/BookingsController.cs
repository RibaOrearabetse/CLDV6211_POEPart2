using EventEaseWebApp.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventEaseWebApp.Models;


namespace EventEaseWebApp.Controllers
{
    public class BookingsController : Controller
    {
        private readonly EventEaseDbContext _context;

        public BookingsController(EventEaseDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public IActionResult Index()
        {
            var bookings = _context.Bookings.Include(b => b.Event).ThenInclude(e => e.Venue).ToList();
            return View(bookings);
        }

        // GET: Bookings/Add
        public async Task<IActionResult> Add()
        {
            var viewModel = new AddBookingViewModel
            {
                Events = await _context.Events.ToListAsync(),
                Venues = await _context.Venues.ToListAsync()
            };

            return View(viewModel);
        }

        // POST: Bookings/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddBookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Events = await _context.Events.ToListAsync();
                model.Venues = await _context.Venues.ToListAsync();
                return View(model);
            }

            bool exists = await _context.Bookings.AnyAsync(b =>
                b.VenueId == model.VenueId &&
                b.BookingDate.Date == model.BookingDate.Date);

            if (exists)
            {
                ModelState.AddModelError("", "Venue is already booked for this date.");
                model.Events = await _context.Events.ToListAsync();
                model.Venues = await _context.Venues.ToListAsync();
                return View(model);
            }

            var booking = new Booking
            {
                EventId = model.EventId,
                VenueId = model.VenueId,
                BookingDate = model.BookingDate
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName");
            return View();
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            bool exists = await _context.Bookings
                .AnyAsync(b => b.VenueId == booking.VenueId && b.BookingDate == booking.BookingDate);

            if (exists)
            {
                ModelState.AddModelError("", "This venue is already booked for that date.");
                return View(booking);
            }

            _context.Add(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            var bookings = from b in _context.Bookings
                           join v in _context.Venues on b.VenueId equals v.VenueId
                           join e in _context.Events on b.EventId equals e.EventId
                           select new
                           {
                               b.BookingId,
                               b.BookingDate,
                               v.VenueName,
                               e.EventName
                           };

            if (!string.IsNullOrEmpty(searchTerm))
            {
                bookings = bookings.Where(b =>
                    b.BookingId.ToString().Contains(searchTerm) ||
                    b.EventName.Contains(searchTerm));
            }

            ViewBag.BookingDetails = await bookings.ToListAsync();

            return View();
        }


    }
}

