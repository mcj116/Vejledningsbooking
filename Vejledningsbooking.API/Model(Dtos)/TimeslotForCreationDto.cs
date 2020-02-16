using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vejledningsbooking.API.Model_Dtos_
{
    public class TimeSlotForCreationDto 
    {


        public DateTime TimeSlotStartDateTime { get; set; }

        public DateTime TimeSlotEndDateTime { get; set; }
        public Guid TeacherId { get; set; }

    }
}
