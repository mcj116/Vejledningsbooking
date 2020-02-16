using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Vejledningsbooking.API.Entities
{
    public class TimeSlot
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime TimeSlotStartDateTime { get; set; }

        [Required]
        public DateTime TimeSlotEndDateTime { get; set; }

        public ICollection<Booking> Bookings { get; set; }
            = new List<Booking>();

        public Calendar Calendar { get; set; }

        public Guid CalendarId { get; set; }
        //public Guid TeacherId { get; set; } TODO:

        public Teacher Teacher { get; set; }
        public Guid TeacherId { get; set; }

    }
}
