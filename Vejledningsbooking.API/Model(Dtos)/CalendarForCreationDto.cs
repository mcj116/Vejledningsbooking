using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vejledningsbooking.API.Entities;

namespace Vejledningsbooking.API.Model_Dtos_
{
    public class CalendarForCreationDto
    {


        public string Name { get; set; }


        public ICollection<TimeSlot> TimeSlots { get; set; }
//= new List<Course>();
    }
}
