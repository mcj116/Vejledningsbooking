using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vejledningsbooking.API.ValidationAttributes;

namespace Vejledningsbooking.API.Entities
{
    [BookingMustBeWithinTimeSlot(
          ErrorMessage = "Student can not have more than two scheduled bookings.")]
    public class Booking : IValidatableObject
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
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (StartDateTime < DateTime.Now)
            {
                results.Add(new ValidationResult("Start date and time must be greater than current time", new[] { "StartDateTime" }));
            }

            if (EndDateTime <= StartDateTime)
            {
                results.Add(new ValidationResult("EndDateTime must be greater that StartDateTime", new[] { "EndDateTime" }));
            }

            return results;
        }
    }
}
