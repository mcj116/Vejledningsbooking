using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vejledningsbooking.API.Entities;

namespace Vejledningsbooking.API.Model_Dtos_
{/// <summary>
/// https://docs.microsoft.com/en-us/aspnet/web-api/overview/data/using-web-api-with-entity-framework/part-5
/// </summary>
    public class CalendarDto
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<TimeSlotDto> TimeSlots { get; set; }
        public List<CourseDto> Courses { get; set; }

    }
}
