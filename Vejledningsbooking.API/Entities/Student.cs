using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vejledningsbooking.API.Entities
{
    public class Student
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string StudentFirstName { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string StudentLastName { get; set; }

        public List<Course> Courses { get; set; }

        public List<Booking> Bookings { get; set; }
    }
}
