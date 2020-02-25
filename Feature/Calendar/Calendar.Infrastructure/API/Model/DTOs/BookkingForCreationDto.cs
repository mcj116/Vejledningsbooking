using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vejledningsbooking.API.Model_Dtos_
{
    public class BookingForCreationDto 
    {


        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }
        public Guid StudentId { get; set; }

    }
}
