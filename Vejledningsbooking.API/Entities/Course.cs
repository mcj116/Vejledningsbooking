using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Vejledningsbooking.API.Entities
{
    public class Course
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }


        //[ForeignKey("CalendarId")]
        public Calendar Calendar { get; set; }

        public Guid CalendarId { get; set; }



        public Guid TeacherId { get; set; }
        //[ForeignKey("TeacherId")]
        public Teacher Teacher { get; set; }


        //[ForeignKey("StudentId")]
        public Student Student { get; set; }

        public Guid StudentId { get; set; } 
            //= new List<Student>();

    }
}
