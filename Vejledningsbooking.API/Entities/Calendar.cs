using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Vejledningsbooking.API.Model_Dtos_;

namespace Vejledningsbooking.API.Entities
{
    public class Calendar
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }


        public List<TimeSlot> TimeSlots { get; set; }

        public List<Course> Courses { get; set; }
        //    = new List<Course>();

    }

}
