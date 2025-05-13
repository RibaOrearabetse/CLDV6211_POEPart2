using EventEaseWebApp.Models.Entities;

namespace EventEaseWebApp.Models
{
    public class AddEventViewModel
    {
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string? Description { get; set; }
        public int? VenueId { get; set; }         // Nullable foreign key

        // Optional navigation property (use if using EF Core)
        public Venue? Venue { get; set; }
    
}
}
