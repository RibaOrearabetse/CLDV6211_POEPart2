using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventEaseWebApp.Models.Entities;

namespace EventEaseWebApp.Models
{
    public class AddBookingViewModel
    {
        [Required]
        public int EventId { get; set; }

        [Required]
        public int VenueId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }

        public List<Event>? Events { get; set; }
        public List<Venue>? Venues { get; set; }
    }
}
