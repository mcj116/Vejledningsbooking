﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vejledningsbooking.API.Entities;

namespace Vejledningsbooking.API.Model_Dtos_
{/// <summary>
/// https://docs.microsoft.com/en-us/aspnet/web-api/overview/data/using-web-api-with-entity-framework/part-5
/// </summary>
    public class TimeSlotDto
    {

        public Guid Id { get; set; }
        public DateTime TimeSlotStartDateTime { get; set; }

        public DateTime TimeSlotEndDateTime { get; set; }

        public Guid TeacherId { get; set; }
        public string TeacherName { get; set; }


        public List<BookingDto> Bookings { get; set; }
    }
}
