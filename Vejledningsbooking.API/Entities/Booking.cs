using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vejledningsbooking.API.Entities
{
    public class Booking
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }


        [ForeignKey("TimeSlotId")]
        public TimeSlot TimeSlot { get; set; }

        public Guid TimeSlotId { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }

    }
}
