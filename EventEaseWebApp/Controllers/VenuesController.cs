using Azure.Storage.Blobs;
using EventEaseWebApp.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEaseWebApp.Controllers
{
    public class VenuesController : Controller
    {
        private readonly EventEaseDbContext _context;
        private readonly BlobService _blobService;

        public VenuesController(EventEaseDbContext context, BlobService blobService)
        {
            _context = context;
            _blobService = blobService;
        }
        private readonly BlobContainerClient _containerClient;

        public VenuesController(IConfiguration configuration)
        {
            var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            var containerName = configuration["AzureBlobStorage:ContainerName"];

            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException(nameof(containerName), "Azure Blob container name is not set in configuration.");
            }

            _containerClient = new BlobContainerClient(connectionString, containerName);
        }

        // GET: Venues
        public IActionResult Index()
        {
            var venues = _context.Venues.ToList();
            return View(venues);
        }

        // GET: Venues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Venues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venue venue, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    venue.ImageUrl = await _blobService.UploadFileAsync(Image);
                }

                _context.Add(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }


        // GET: Venues/Details/5
        public IActionResult Details(int id)
        {
            var venue = _context.Venues.FirstOrDefault(v => v.VenueId == id);
            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hasBookings = await _context.Bookings.AnyAsync(b => b.VenueId == id);
            if (hasBookings)
            {
                TempData["Error"] = "You cannot delete this venue. It has active bookings.";
                return RedirectToAction(nameof(Index));
            }

            var venue = await _context.Venues.FindAsync(id);
            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
