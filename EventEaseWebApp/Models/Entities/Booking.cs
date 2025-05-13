using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEaseWebApp.Models.Entities
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public int VenueId { get; set; } // ✅ This is required

        [ForeignKey("VenueId")]
        public Venue Venue { get; set; }

        [Required]
        public int EventId { get; set; }

        [ForeignKey("EventId")]
        public Event Event { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        // You can add other properties like TimeSlot, UserEmail, etc.
    }
}
