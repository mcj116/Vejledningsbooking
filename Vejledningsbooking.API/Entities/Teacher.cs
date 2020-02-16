using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vejledningsbooking.API.Entities
{
    public class Teacher
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string TeacherFirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string TeacherLastName { get; set; }
        //public ICollection<Course> Courses { get; set; }
        //= new List<Course>();
        public List<Course> Courses { get; set; }
        public List<TimeSlot> TimeSlots { get; set; }

    }
}

